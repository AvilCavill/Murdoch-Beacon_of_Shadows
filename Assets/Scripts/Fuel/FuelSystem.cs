using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    public PlayerInventory playerInventory; // Referencia al inventario del jugador
    public LevelManager levelManager; // Referencia al gestor del nivel
    public float fuelTime = 10f; // Tiempo adicional que cada madera añade
    public KeyCode useFuelKey = KeyCode.F; // Tecla para usar madera como combustible
    public string woodItemName = "Wood"; // Nombre del item de madera

    public GameObject addWoodText;
    public GameObject noWoodText;
    private bool isPlayerInZone = false; // Indica si el jugador está dentro del área

    private void Start()
    {
        addWoodText.SetActive(false);
        noWoodText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            addWoodText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            addWoodText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(useFuelKey))
        {
            UseFuel();
        }
    }

    private void UseFuel()
    {
        // Verifica si el jugador tiene madera en el inventario
        ItemSO woodItem = playerInventory.inventory.Find(item => item.itemName == woodItemName);

        if (woodItem != null)
        {
            // Eliminar la madera del inventario
            playerInventory.inventory.Remove(woodItem);
            playerInventory.UpdateHotbarUI();

            // Añadir tiempo al faro
            levelManager.AddTime(fuelTime);
            
        }
        else
        {
            noWoodText.SetActive(true);
            addWoodText.SetActive(false);
            
            //Corutina para hacer fade del texto
            StartCoroutine(HideText(3f));
        }
    }

    private IEnumerator HideText(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        noWoodText.SetActive(false);

        if (isPlayerInZone)
        {
            addWoodText.SetActive(true);
        }
    }
}