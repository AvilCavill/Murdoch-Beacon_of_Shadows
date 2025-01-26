using UnityEngine;

namespace Items.CameraUV
{
    public class UVCamera : MonoBehaviour
    {
        public GameObject uvCameraObject; // Cámara UV
        public GameObject flashlightObject; // Linterna
        public int maxUses = 5; // Máximo número de usos
        public int currentUses; // Número actual de usos
        public float flashRange = 10f; // Alcance del flash UV
        public float stunDuration = 3f; // Duración del aturdimiento
        public KeyCode flashKey = KeyCode.F; // Tecla para usar el flash UV
        public KeyCode switchKey = KeyCode.T; // Tecla para cambiar entre cámara UV y linterna
        public KeyCode rechargeKey = KeyCode.R;
        public string batteryItemName = "BatteryUV"; // Nombre del ítem batería
        public PlayerInventory playerInventory; // Referencia al inventario del jugador
        public bool hasUVCamera = false;

        private bool isUVCameraActive = true; // Indica si la cámara UV está activa

        void Start()
        {
            currentUses = maxUses; // Inicializa con el máximo de usos
            ActivateUVCamera();
        }

        void Update()
        {
            // Cambiar entre cámara UV y linterna
            if (Input.GetKeyDown(switchKey) && hasUVCamera)
            {
                ToggleDevice();
            }

            // Usar el flash UV
            if (isUVCameraActive && Input.GetKeyDown(flashKey) && currentUses > 0)
            {
                UseFlash();
            }

            if (isUVCameraActive && hasUVCamera && Input.GetKeyDown(rechargeKey) )
            {
                Recharge();
            }
        }

        void ToggleDevice()
        {
            isUVCameraActive = !isUVCameraActive;

            if (isUVCameraActive)
            {
                ActivateUVCamera();
            }
            else
            {
                ActivateFlashlight();
            }
        }

        void ActivateUVCamera()
        {
            isUVCameraActive = true;
            uvCameraObject.SetActive(true);
            flashlightObject.SetActive(false);
            Debug.Log("Cámara UV activada.");
        }

        void ActivateFlashlight()
        {
            isUVCameraActive = false;
            flashlightObject.SetActive(true);
            Debug.Log("Linterna activada.");
        }

        void UseFlash()
        {
            GetComponent<CameraFlash>().TriggerFlash();
            // Reducir usos y mostrar el efecto del flash
            currentUses--;

            // Buscar enemigos en el rango del flash UV
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, flashRange);
            foreach (var hitCollider in hitColliders)
            {
                IABehaviour enemyAI = hitCollider.GetComponent<IABehaviour>();
                if (enemyAI != null)
                {
                    enemyAI.Stun(3f); // Aturdir por 3 segundos
                }
            }

            Debug.Log("Flash UV usado. Usos restantes: " + currentUses);
        }

        void StunEnemy(GameObject enemy)
        {
            IABehaviour enemyController = enemy.GetComponent<IABehaviour>();
            if (enemyController != null)
            {
                enemyController.Stun(stunDuration);
            }
        }

        public void Recharge()
        {
            // Intentar encontrar una batería del tipo BatteryUV en el inventario
            int batteryUVIndex = playerInventory.inventory.FindIndex(item => item != null && item.itemType == ItemType.BatteryUV);

            if (batteryUVIndex != -1 && currentUses < maxUses)
            {
                // Recargar la cámara UV (sumar un uso)
                currentUses = Mathf.Min(currentUses + 1, maxUses);

                // Remover la batería del inventario
                playerInventory.inventory.RemoveAt(batteryUVIndex);

                // Actualizar la UI del inventario
                playerInventory.UpdateHotbarUI();

                Debug.Log("Cámara UV recargada. Usos actuales: " + currentUses);
            }
            else
            {
                Debug.Log("No hay baterías disponibles o la cámara UV ya está llena.");
            }
        }

    }
}
