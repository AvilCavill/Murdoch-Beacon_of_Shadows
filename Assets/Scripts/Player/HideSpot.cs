using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    public Transform hidePoint; // Punto donde el jugador se esconde
    public float exitOffset = 1.5f; // Distancia delante del armario al salir
    private bool isPlayerNear = false;
    public GameObject hideText;

    private void Start()
    {
        hideText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            hideText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            hideText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
         
            if (player.IsHiding())
            {
                // Salir del escondite
                player.SetHiding(false);

                // Calcular posición delante del armario
                Vector3 exitPosition = hidePoint.position + (transform.forward * exitOffset);
                exitPosition.y = player.transform.position.y; // Mantener altura actual del jugador

                // Mover al jugador
                player.transform.position = exitPosition;

                hideText.SetActive(false);
            }
            else
            {
                // Entrar al escondite
                player.transform.position = hidePoint.position;
                player.SetHiding(true);
            }
        }
    }
}