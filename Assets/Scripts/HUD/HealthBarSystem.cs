using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSystem : MonoBehaviour
{
    public Image fill;
    public Image background;
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;
    public HUDController hudController;

    void Start()
    {
        hudController = FindObjectOfType<HUDController>();
        fill.enabled = false;
        background.enabled = false;
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
    }

    public void TakeDamage(float damage)
    {
        hudController.ShowDamage();
        fill.enabled = true;
        background.enabled = true;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Asegurarse de que la vida no sea negativa
        healthSlider.value = health;

        // Reinicia el temporizador de desactivación.
        CancelInvoke("HideHealthBar");
        Invoke("HideHealthBar", 5f);
    }

    public void Heal(float amount)
    {
        fill.enabled = true;
        background.enabled = true;
        StartCoroutine(HealOverTime(amount));

        // Reinicia el temporizador de desactivación.
        CancelInvoke("HideHealthBar");
        Invoke("HideHealthBar", 5f);
    }
    
    private IEnumerator HealOverTime(float amount)
    {
        float healPerFrame = amount / 20f; // Dividir la curación en pequeños incrementos (ajustable)
        float targetHealth = Mathf.Clamp(health + amount, 0, maxHealth); // Salud final deseada

        while (health < targetHealth)
        {
            health += healPerFrame;
            health = Mathf.Clamp(health, 0, maxHealth); // Asegurarse de no superar el máximo
            healthSlider.value = health; // Actualizar la barra visualmente
            yield return new WaitForSeconds(0.05f); // Pausa breve entre incrementos
        }

        health = targetHealth; // Asegurarse de llegar al valor exacto
        healthSlider.value = health; // Actualizar visualmente
    }

    private void HideHealthBar()
    {
        fill.enabled = false;
        background.enabled = false;
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth * 100.0f;
    }
}