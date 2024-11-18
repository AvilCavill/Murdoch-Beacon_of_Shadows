using UnityEngine;
using UnityEngine.UI;

public class HealthBarSystem : MonoBehaviour
{
    public Image fill;
    public Image background;
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float health;

    void Start()
    {
        fill.enabled = false;
        background.enabled = false;
        healthSlider.maxValue = maxHealth;
        health = maxHealth;
        healthSlider.value = health;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(float damage)
    {
        fill.enabled = true;
        background.enabled = true;
        health -= damage;
        healthSlider.value = health;

        // Reinicia el temporizador de desactivación.
        CancelInvoke("HideHealthBar");
        Invoke("HideHealthBar", 5f);
    }

    private void HideHealthBar()
    {
        fill.enabled = false;
        background.enabled = false;
    }
}