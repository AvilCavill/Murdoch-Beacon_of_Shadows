using UnityEngine;

public class ItemPickable : MonoBehaviour
{
    public ItemSO itemData; // Datos del objeto (tipo, sprite, etc.)

    public void PickItem(Transform handTransform)
    {
        //Añadimos el objeto como hijo de Hand del Player
        GameObject itemInstance = Instantiate(itemData.itemPrefab, handTransform);
        Destroy(gameObject);
    }
}