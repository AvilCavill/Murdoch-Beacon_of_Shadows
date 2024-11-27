using UnityEngine;

public class LogPickup : MonoBehaviour
{
    public float timeToAdd = 3f; // Tiempo que añade este tronco
    public delegate void LogDestroyed();
    public event LogDestroyed OnDestroyed; // Evento que se lanza al destruirse

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.AddTime(timeToAdd);
            }

            OnDestroyed?.Invoke(); // Lanza el evento
            Destroy(gameObject); // Destruye el tronco
        }
    }
}