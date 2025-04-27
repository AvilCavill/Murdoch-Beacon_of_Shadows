 using System.Collections.Generic;
using UnityEngine;

namespace Items.Spawners
{
    public class SpawnerBehaviour : MonoBehaviour
    {
        public GameObject logPrefab; // Prefab del tronco
        public int maxLogs = 5; // Número máximo de troncos en el área
        public float spawnRadius = 5f; // Radio del área de generación
        public float spawnInterval = 3f; // Tiempo entre intentos de generación

        private List<GameObject> spawnedLogs = new List<GameObject>(); // Lista de troncos generados

        void Start()
        {
            // Inicia un bucle de generación repetido
            InvokeRepeating(nameof(SpawnLog), 1f, spawnInterval);
        }

    
        void SpawnLog()
        {
            // Limpia cualquier referencia nula
            spawnedLogs.RemoveAll(log => log == null);

            // Comprueba si ya se ha alcanzado el máximo de troncos
            if (spawnedLogs.Count >= maxLogs)
            {
                return;
            }

            // Genera una posición aleatoria dentro de un área circular en 2D
            Vector2 randomPos = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(transform.position.x + randomPos.x, transform.position.y, transform.position.z + randomPos.y);

            // Instancia el tronco
            GameObject newLog = Instantiate(logPrefab, spawnPosition, Quaternion.identity);

            // Se registra para eliminarlo al recogerlo
            if (newLog.TryGetComponent<ItemPickable>(out var pickable))
            {
                pickable.OnItemPickedUp += RemoveLog;
            }

            // Añade el tronco a la lista
            spawnedLogs.Add(newLog);
        }


        void RemoveLog(GameObject log)
        {
            if (spawnedLogs.Contains(log))
            {
                spawnedLogs.Remove(log);
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