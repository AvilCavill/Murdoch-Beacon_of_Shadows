using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    //Variables Stamina
    public float playerStamina = 100.0f;
    private float playerMaxStamina = 100.0f;
    public bool hasRegenerated = true;
    public bool playerIsSprinting = false;
    
    //Parametros Regen Stamina
    private float staminaDrain = 0.5f;
    private float staminaRegen = 0.5f;
    
    //Variables UI Stamina
    private Image staminaProgressUI = null;
    private CanvasGroup sliderCanvasGroup = null;
    
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerIsSprinting)
        {
            if (playerStamina <= playerMaxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;
                UpdateStamina(1);

                if (playerStamina >= playerMaxStamina)
                {
                    sliderCanvasGroup.alpha = 0;
                    hasRegenerated = true;
                }
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            playerIsSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0;
            }
        }
    }
    
    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / playerMaxStamina;
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}