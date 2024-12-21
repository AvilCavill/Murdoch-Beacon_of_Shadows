using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public ItemSO itemData; // Datos del objeto (tipo, sprite, etc.)

    public void PickItem()
    {
        Destroy(gameObject); // Destruir el objeto del mundo al recogerlo
    }
}