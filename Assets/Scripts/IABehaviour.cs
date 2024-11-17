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

    [Header("Componentes")]
    private NavMeshAgent agente;
    private bool persiguiendo = false;

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
                persiguiendo = false;
                agente.SetDestination(puntosDePatrulla[indiceActual].position);
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
                float angulo = Vector3.Angle(transform.forward, direccionAlJugador);
                if (angulo <= anguloVision / 2f)
                {
                    // Verifica si hay línea de visión clara
                    if (!Physics.Raycast(transform.position, direccionAlJugador, distanciaAlJugador, obstaculos))
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
        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);
        return angulo <= anguloVision / 2f && !Physics.Raycast(transform.position, direccionAlJugador, Vector3.Distance(transform.position, jugador.position), obstaculos);
    }
}
