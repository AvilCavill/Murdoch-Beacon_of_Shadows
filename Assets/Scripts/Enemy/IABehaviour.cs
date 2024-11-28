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
    public float rangoVision = 10f; // Distancia de visión
    public float anguloVision = 60f; // Ángulo de visión
    public LayerMask capaJugador; // Capa del jugador
    public LayerMask obstaculos; // Capa de los obstáculos
    public float tiempoParaPerderJugador = 2f; // Tiempo antes de abandonar la persecución

    //Animaciones
    private Animator _animator;
    
    [Header("Componentes")]
    private NavMeshAgent agente;
    private bool persiguiendo = false;
    private float tiempoSinVerJugador = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (puntosDePatrulla.Length > 0)
        {
            agente.SetDestination(puntosDePatrulla[indiceActual].position);
        }
    }

    void Update()
    {
        if (persiguiendo)
        {
            PerseguirJugador();
        }
        else
        {
            Patrullar();
            DetectarJugador();
        }
    }

    void Patrullar()
    {
        // Verifica si hay puntos de patrulla configurados
        if (puntosDePatrulla.Length == 0) return;

        // Verifica si llegó al punto de patrulla actual
        if (!agente.pathPending && agente.remainingDistance < 0.5f)
        {
            indiceActual = (indiceActual + 1) % puntosDePatrulla.Length;
            agente.SetDestination(puntosDePatrulla[indiceActual].position);
        }
    }

    void PerseguirJugador()
    {
        if (jugador != null)
        {
            
            agente.SetDestination(jugador.position);

            // Verifica si el jugador salió del campo de visión
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador > rangoVision || !EstaEnCampoDeVision())
            {
                tiempoSinVerJugador += Time.deltaTime;

                if (tiempoSinVerJugador >= tiempoParaPerderJugador)
                {
                    persiguiendo = false;
                    tiempoSinVerJugador = 0f;
                    agente.SetDestination(puntosDePatrulla[indiceActual].position);
                }
            }
            else
            {
                tiempoSinVerJugador = 0f; // Resetea el temporizador si el jugador está visible
            }
        }
    }

    void DetectarJugador()
    {
        if (jugador != null)
        {
            Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            // Verifica si el jugador está dentro del rango y del ángulo de visión
            if (distanciaAlJugador <= rangoVision)
            {
                direccionAlJugador.y = 0; // Ignora la altura
                float angulo = Vector3.Angle(transform.forward, direccionAlJugador);
                if (angulo <= anguloVision / 2f)
                {
                    // Verifica si hay línea de visión clara
                    Vector3 origenRaycast = transform.position + Vector3.up * 1.5f; // Eleva el rayo
                    if (!Physics.Raycast(origenRaycast, direccionAlJugador, distanciaAlJugador, obstaculos))
                    {
                        persiguiendo = true;
                    }
                }
            }
        }
    }

    bool EstaEnCampoDeVision()
    {
        if (jugador == null) return false;

        Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
        direccionAlJugador.y = 0; // Ignora la altura
        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

        Vector3 origenRaycast = transform.position + Vector3.up * 1.5f; // Eleva el rayo
        return angulo <= anguloVision / 2f &&
               !Physics.Raycast(origenRaycast, direccionAlJugador, Vector3.Distance(transform.position, jugador.position), obstaculos);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de visión
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoVision);

        // Dibuja el ángulo de visión
        Vector3 anguloIzquierdo = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;
        Vector3 anguloDerecho = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, anguloIzquierdo * rangoVision);
        Gizmos.DrawRay(transform.position, anguloDerecho * rangoVision);
    }
}
