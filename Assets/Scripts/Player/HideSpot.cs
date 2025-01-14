using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
    public Transform hidePoint; // Punto donde el jugador se esconde
    public Transform exitArea; // Área donde el jugador aparece al salir del escondite
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
            // Obtener referencia al jugador
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            
            if (player.IsHiding())
            {
                // Salir del escondite
                player.SetHiding(false);

                // Mover al jugador al área de salida
                if (exitArea != null)
                {
                    player.transform.position = exitArea.position;
                }
                else
                {
                    Debug.LogWarning("ExitArea no está asignado en el HideSpot.");
                }

                hideText.SetActive(true); // Mostrar el texto nuevamente
            }
            else
            {
                // Entrar al escondite
                player.transform.position = hidePoint.position;
                player.SetHiding(true);

                hideText.SetActive(false); // Ocultar el texto
            }
        }
    }
}