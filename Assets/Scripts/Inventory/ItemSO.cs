using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName; // Nombre del objeto
    public Sprite itemSprite; // Sprite para la hotbar
    public ItemType itemType; // Tipo del objeto
}

public enum ItemType
{
    Wood,
    Medkit
}