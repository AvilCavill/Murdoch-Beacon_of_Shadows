using System;
using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public ItemSO itemData; // Datos del objeto (tipo, sprite, etc.)
    public event Action<GameObject> OnItemPickedUp;

    public void PickItem(Transform handTransform)
    {
        OnItemPickedUp?.Invoke(gameObject);
        //Añadimos el objeto como hijo de Hand del Player
        GameObject itemInstance = Instantiate(itemData.itemPrefab, handTransform);
        Destroy(gameObject);
    }
}