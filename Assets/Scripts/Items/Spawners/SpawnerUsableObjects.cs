using System.Collections.Generic;
using UnityEngine;

namespace Items.Spawners
{
    public class SpawnerUsableObjects : MonoBehaviour
    {
        public GameObject medkitPrefab; // Prefab del medkit
        public GameObject batteryCellPrefab; // Prefab de la bateria
        public int maxItems = 2; // Número máximo de items en el área
        public float spawnRadius = 5f; // Radio del área de generación
        public float spawnInterval = 3f; // Tiempo entre intentos de generación

        private List<GameObject> spawnedItems = new List<GameObject>(); 

        void Start()
        {
            // Inicia un bucle de generación repetido
            InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
        }

    
        void SpawnItem()
        {
            // Comprueba si ya se ha alcanzado el máximo de items
            if (spawnedItems.Count >= maxItems)
            {
                return;
            }

            // Genera una posición aleatoria dentro de un área circular
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
            spawnPosition.y = transform.position.y; // Mantén la posición en el mismo nivel

            GameObject prefabToSpawn = Random.value > 0.5f ? medkitPrefab : batteryCellPrefab;
            
            // Crea el objeto   
            GameObject newItem = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

            ItemPickable itemPickable = newItem.GetComponent<ItemPickable>();
            if (itemPickable != null)
            {
                itemPickable.OnItemPickedUp += RemoveItem;
            }
            
            // Añade el item a la lista
            spawnedItems.Add(newItem);
        }

        void RemoveItem(GameObject item)
        {
            if (spawnedItems.Contains(item))
            {
                spawnedItems.Remove(item);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Dibuja el área de generación en la vista de escena
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }

}
