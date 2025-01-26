using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IABehaviour : MonoBehaviour
{
    [Header("Patrulla")]
    public Transform[] puntosDePatrulla; // Puntos de patrulla
    private int indiceActual = 0; // Índice del punto de patrulla actual

    [Header("Persecución")]
    public Transform jugador; // Referencia al jugador
    public float rangoVision = 40f; // Distancia de visión
    public float anguloVision = 70f; // Ángulo de visión
    public LayerMask capaJugador; // Capa del jugador
    public LayerMask obstaculos; // Capa de los obstáculos
    public float tiempoParaPerderJugador = 5f; // Tiempo antes de abandonar la persecución
    public float distanciaAtaque = 3f; // Distancia mínima para atacar al jugador
    public float tiempoEntreAtaques = 3f; // Tiempo entre ataques

    [Header("Componentes")]
    private NavMeshAgent agente;
    private Animator animator; // Componente Animator
    private bool persiguiendo = false;
    private bool atacando = false;
    private float tiempoSinVerJugador = 0f;
    private float temporizadorAtaque = 0f;
    public HealthBarSystem healthBarSystem;
    
    [Header("Stun")]
    private bool isStunned = false;
    private float stunTimer = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("Patrol", true);
        
        if (puntosDePatrulla.Length > 0)
        {
            agente.SetDestination(puntosDePatrulla[indiceActual].position);
            
        }
    }

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
                EnableEnemyAI(); // Restaura el comportamiento normal
            }
            return; // No ejecutar más lógica si el enemigo está aturdido
        }

        temporizadorAtaque += Time.deltaTime;

        if (jugador != null && jugador.GetComponent<PlayerController>().IsHiding())
        {
            persiguiendo = false;
            agente.SetDestination(puntosDePatrulla[indiceActual].position);
            animator.SetBool("Patrol", true);
        }

        if (persiguiendo)
        {
            if (Vector3.Distance(transform.position, jugador.position) <= distanciaAtaque)
            {
                AtacarJugador();
            }
            else
            {
                animator.SetBool("PlayerIsNear", false);
                PerseguirJugador();
            }
        }
        else
        {
            Patrullar();
            DetectarJugador();
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        // Desactivar comportamiento del enemigo mientras está aturdido
        DisableEnemyAI();
        Debug.Log("Enemigo aturdido por " + duration + " segundos.");
    }

    private void DisableEnemyAI()
    {
        agente.isStopped = true;
        animator.SetBool("Stunned", true);
    }
    
    private void EnableEnemyAI()
    {
        agente.isStopped = false;
        animator.SetBool("Stunned", false);
    }


    void Patrullar()
    {
        if (puntosDePatrulla.Length == 0) return;

        if (!agente.pathPending && agente.remainingDistance < 0.5f)
        {
            indiceActual = (indiceActual + 1) % puntosDePatrulla.Length;
            agente.SetDestination(puntosDePatrulla[indiceActual].position);
        }

        // Cambiar animación a "Patrulla" si no está persiguiendo ni atacando
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Creep_Crouch_Action"))
        {
            animator.SetBool("Patrol", true);
        }
    }

    void PerseguirJugador()
    {
        if (jugador != null)
        {
            agente.SetDestination(jugador.position);

            // Cambia la animación a "Correr" o "Perseguir"
            if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Creep_Crouch_Action"))
            {
                animator.SetTrigger("Pursue");
            }

            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador > rangoVision || !EstaEnCampoDeVision())
            {
                tiempoSinVerJugador += Time.deltaTime;

                if (tiempoSinVerJugador >= tiempoParaPerderJugador)
                {
                    persiguiendo = false;
                    tiempoSinVerJugador = 0f;
                    agente.SetDestination(puntosDePatrulla[indiceActual].position);

                    // Vuelve a la animación de patrulla
                    animator.SetBool("Patrol", true);
                }
            }
            else
            {
                tiempoSinVerJugador = 0f;
            }
        }
    }

    void AtacarJugador()
    {
        animator.SetBool("Patrol", false);
        if (temporizadorAtaque >= tiempoEntreAtaques)
        {
            temporizadorAtaque = 0f; // Reinicia el temporizador

            // // Cambia a la animación de ataque
            
                animator.SetTrigger("Attack");
                animator.SetTrigger("Attack");
                animator.SetBool("PlayerIsNear", true);
            

            // Lógica de daño
            
            if (healthBarSystem.health != null)
            {
                healthBarSystem.TakeDamage(20);
            }
        }
    }

    void DetectarJugador()
    {
        if (jugador != null)
        {
            Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= rangoVision)
            {
                direccionAlJugador.y = 0;
                float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

                if (angulo <= anguloVision / 2f)
                {
                    Vector3 origenRaycast = transform.position + Vector3.up * 1.5f;
                    if (!Physics.Raycast(origenRaycast, direccionAlJugador, distanciaAlJugador, obstaculos))
                    {
                        persiguiendo = true;

                        // Cambia a la animación de persecución
                        animator.SetTrigger("Pursue");
                    }
                }
            }
        }
    }

    bool EstaEnCampoDeVision()
    {
        if (jugador == null) return false;

        Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
        direccionAlJugador.y = 0;
        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

        Vector3 origenRaycast = transform.position + Vector3.up * 1.5f;
        return angulo <= anguloVision / 2f &&
               !Physics.Raycast(origenRaycast, direccionAlJugador, Vector3.Distance(transform.position, jugador.position), obstaculos);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        Vector3 anguloIzquierdo = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;
        Vector3 anguloDerecho = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, anguloIzquierdo * rangoVision);
        Gizmos.DrawRay(transform.position, anguloDerecho * rangoVision);
    }
}
