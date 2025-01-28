using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Comprueba si ya se ha alcanzado el máximo de troncos
        if (spawnedLogs.Count >= maxLogs)
        {
            return;
        }

        // Genera una posición aleatoria dentro de un área circular
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = transform.position.y; // Mantén la posición en el mismo nivel

        // Crea el tronco
        GameObject newLog = Instantiate(logPrefab, spawnPosition, Quaternion.identity);

        // Añade el tronco a la lista
        spawnedLogs.Add(newLog);
    }


    private void OnDrawGizmosSelected()
    {
        // Dibuja el área de generación en la vista de escena
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}