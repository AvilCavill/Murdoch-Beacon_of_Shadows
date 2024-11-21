using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarSystem : MonoBehaviour
{
    public Image fill;
    public Image background;
    public Slider staminaSlider;
    public float maxStamina = 100.0f;
    public float stamina;
    public float staminaDrainRate = 15.0f; // Tasa de consumo por segundo
    public float staminaRegenRate = 10.0f; // Tasa de regeneración por segundo
    public KeyCode runKey = KeyCode.LeftShift; // Tecla para correr

    private bool isRunning = false;

    void Start()
    {
        fill.enabled = false;
        background.enabled = false;
        staminaSlider.maxValue = maxStamina;
        stamina = maxStamina;
        staminaSlider.value = stamina;
    }

    void Update()
    {
        // Detectar si el jugador está corriendo
        isRunning = Input.GetKey(runKey) && stamina > 0;

        if (isRunning)
        {
            // Reducir estamina
            stamina -= staminaDrainRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina); // Asegurarse de que no sea menor que 0
            ShowStaminaBar();
        }
        else
        {
            // Regenerar estamina
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina); // Asegurarse de que no exceda el máximo
        }

        // Actualizar la barra de estamina
        staminaSlider.value = stamina;

        // Ocultar la barra si está llena
        if (stamina >= maxStamina)
        {
            Invoke("HideStaminaBar", 3f); // Ocultar después de 3 segundos
        }
    }

    private void ShowStaminaBar()
    {
        fill.enabled = true;
        background.enabled = true;
        CancelInvoke("HideStaminaBar"); // Evitar ocultar la barra mientras se usa
    }

    private void HideStaminaBar()
    {
        fill.enabled = false;
        background.enabled = false;
    }
}