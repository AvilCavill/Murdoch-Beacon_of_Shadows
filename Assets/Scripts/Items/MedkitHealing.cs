using UnityEngine;

public class MedkitHealing : MonoBehaviour
{
    [Header("Configuración de Curación")]
    public KeyCode healKey = KeyCode.H; // Tecla para usar el botiquín
    public int healAmount = 40; // Cantidad de vida que cura el botiquín

    [Header("Sistemas Conectados")]
    public PlayerInventory playerInventory; // Referencia al inventario del jugador
    public HealthBarSystem healthBarSystem; // Referencia al sistema de barra de vida

    void Update()
    {
        HandleHealing();
    }

    private void HandleHealing()
    {
        // Comprueba si se presiona la tecla de curación
        if (Input.GetKeyDown(healKey))
        {
            // Verifica si hay un botiquín en el inventario
            int medkitIndex = playerInventory.inventory.FindIndex(item => item != null && item.itemType == ItemType.Medkit);

            if (medkitIndex != -1 && healthBarSystem.healthSlider.value < healthBarSystem.healthSlider.maxValue)
            {
                // Cura al jugador
                healthBarSystem.Heal(healAmount);
                

                // Remueve el botiquín del inventario
                playerInventory.inventory.RemoveAt(medkitIndex);
                playerInventory.UpdateHotbarUI();

                Debug.Log("El jugador ha sido curado con un botiquín.");
            }
            else
            {
                Debug.Log("No hay botiquines disponibles o la vida está completa.");
            }
        }
    }
}