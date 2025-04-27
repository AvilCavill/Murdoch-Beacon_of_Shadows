using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public enum EnemyState
    {
        Patrolling,
        Pursuing,
        Attacking,
        Stunned
    }

    public class IABehaviour : MonoBehaviour
    {
        [Header("Patrulla")]
        public Transform[] puntosDePatrulla;
        private int indiceActual = 0;

        [Header("Persecución")]
        public Transform jugador;
        public float rangoVision = 40f;
        public float anguloVision = 70f;
        public LayerMask capaJugador;
        public LayerMask obstaculos;
        public float tiempoParaPerderJugador = 5f;
        public float distanciaAtaque = 3f;
        public float tiempoEntreAtaques = 3f;

        [Header("Componentes")]
        private NavMeshAgent agente;
        private Animator animator;
        private float tiempoSinVerJugador = 0f;
        private float temporizadorAtaque = 0f;

        [Header("Vida")]
        public HealthBarSystem healthBarSystem;

        [Header("Stun")]
        private bool isStunned = false;
        private float stunTimer = 0f;

        private EnemyState currentState = EnemyState.Patrolling;
        
        private PlayerController jugadorController;

        void Start()
        {
            agente = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            if (jugador != null)
                jugadorController = jugador.GetComponent<PlayerController>();
            
            if (puntosDePatrulla.Length > 0)
                agente.SetDestination(puntosDePatrulla[indiceActual].position);

            UpdateAnimationState();
        }

        void Update()
        {
            if (isStunned)
            {
                UpdateStun();
                return;
            }

            temporizadorAtaque += Time.deltaTime;

            switch (currentState)
            {
                case EnemyState.Patrolling:
                    Patrullar();
                    DetectarJugador();
                    break;

                case EnemyState.Pursuing:
                    PerseguirJugador();
                    break;

                case EnemyState.Attacking:
                    AtacarJugador();
                    break;
            }
        }

        void UpdateAnimationState()
        {
            animator.SetInteger("State", (int)currentState);
        }

        void UpdateStun()
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
                agente.isStopped = false;
                ChangeState(EnemyState.Patrolling);
            }
        }

        public void Stun(float duration)
        {
            isStunned = true;
            stunTimer = duration;
            agente.isStopped = true;
            ChangeState(EnemyState.Stunned);
        }

        void ChangeState(EnemyState newState)
        {
            currentState = newState;
            UpdateAnimationState();
        }

        void Patrullar()
        {
            if (puntosDePatrulla.Length == 0) return;

            if (!agente.pathPending && agente.remainingDistance < 0.5f)
            {
                indiceActual = (indiceActual + 1) % puntosDePatrulla.Length;
                agente.SetDestination(puntosDePatrulla[indiceActual].position);
            }
        }

        void PerseguirJugador()
        {
            if (jugador == null)
            {
                ChangeState(EnemyState.Patrolling);
                return;
            }

            // Si el jugador está escondido
            if (jugadorController != null && jugadorController.IsHiding())
            {
                agente.SetDestination(transform.position); // Detener al enemigo
                tiempoSinVerJugador += Time.deltaTime;

                if (tiempoSinVerJugador >= tiempoParaPerderJugador)
                {
                    tiempoSinVerJugador = 0f;
                    ChangeState(EnemyState.Patrolling);
                    agente.SetDestination(puntosDePatrulla[indiceActual].position);
                }

                return; // No seguir persiguiendo
            }

            // Si no está escondido, seguir normalmente
            agente.SetDestination(jugador.position);

            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= distanciaAtaque)
            {
                ChangeState(EnemyState.Attacking);
            }
            else if (distanciaAlJugador > rangoVision || !EstaEnCampoDeVision())
            {
                tiempoSinVerJugador += Time.deltaTime;

                if (tiempoSinVerJugador >= tiempoParaPerderJugador)
                {
                    tiempoSinVerJugador = 0f;
                    ChangeState(EnemyState.Patrolling);
                    agente.SetDestination(puntosDePatrulla[indiceActual].position);
                }
            }
            else
            {
                tiempoSinVerJugador = 0f;
            }
        }

        void AtacarJugador()
        {
            if (jugador == null)
            {
                ChangeState(EnemyState.Patrolling);
                return;
            }

            agente.SetDestination(transform.position); // Detiene al enemigo para atacar

            if (temporizadorAtaque >= tiempoEntreAtaques)
            {
                temporizadorAtaque = 0f;
                animator.SetTrigger("Attack");

                if (healthBarSystem.health != null)
                {
                    healthBarSystem.TakeDamage(20);
                }
            }

            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador > distanciaAtaque)
            {
                ChangeState(EnemyState.Pursuing);
            }
        }

        void DetectarJugador()
        {
            if (jugador == null || (jugadorController != null && jugadorController.IsHiding())) return;

            Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= rangoVision)
            {
                direccionAlJugador.y = 0;
                float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

                Vector3 origenRaycast = transform.position + Vector3.up * 1.5f;
                if (angulo <= anguloVision / 2f && !Physics.Raycast(origenRaycast, direccionAlJugador, distanciaAlJugador, obstaculos))
                {
                    ChangeState(EnemyState.Pursuing);
                }
            }
        }

        bool EstaEnCampoDeVision()
        {
            if (jugador == null || (jugadorController != null && jugadorController.IsHiding())) return false;

            Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;
            direccionAlJugador.y = 0;
            float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

            Vector3 origenRaycast = transform.position + Vector3.up * 1.5f;
            return angulo <= anguloVision / 2f &&
                   !Physics.Raycast(origenRaycast, direccionAlJugador, Vector3.Distance(transform.position, jugador.position), obstaculos);
        }
    }
}
