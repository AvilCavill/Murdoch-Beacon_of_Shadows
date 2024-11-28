using UnityEngine;

public class HighlightObject : MonoBehaviour
{
    public Material highlightMaterial;
    public Material defaultMaterial;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectRenderer.material = highlightMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectRenderer.material = defaultMaterial;
        }
    }
}