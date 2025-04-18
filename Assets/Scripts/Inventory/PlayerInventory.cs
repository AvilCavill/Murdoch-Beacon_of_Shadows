using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Configuración General")] public List<ItemSO> inventory = new List<ItemSO>();
    public int maxInventorySize = 4; // Tamaño máximo de la hotbar
    public int selectedIndex = 0; // Índice del objeto seleccionado

    [Header("Configuración de Entrada")] public KeyCode throwItemKey = KeyCode.Q; // Tecla para lanzar
    public KeyCode pickItemKey = KeyCode.E; // Tecla para recoger

    [Header("UI de la Hotbar")] public Image[] hotbarSlots; // Imagen de las ranuras de la hotbar
    public Image[] slotBackgrounds;
    public Sprite emptySlotSprite; 
    public Color selectedColor = Color.green; 
    public Color defaultColor = Color.white;
    
    [Header("Configuración de Lanzamiento")]
    public Transform throwPosition; // Posición desde donde se lanzará el objeto

    public GameObject woodPrefab; // Prefab del objeto madera
    public GameObject medkitPrefab; // Prefab del objeto botiquín
    public GameObject batteryUV;
    
    

    private Dictionary<ItemType, GameObject> itemPrefabs;

    void Start()
    {
        // Inicializamos el diccionario de prefabs
        itemPrefabs = new Dictionary<ItemType, GameObject>
        {
            { ItemType.Wood, woodPrefab },
            { ItemType.Medkit, medkitPrefab },
            { ItemType.BatteryUV, batteryUV}
        };

        UpdateHotbarUI(); // Actualizar la UI de la hotbar
    }

    void Update()
    {
        HandlePickup();
        HandleThrow();
        HandleSelection();
    }

    private void HandlePickup()
    {
        if (Input.GetKeyDown(pickItemKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
            {
                ItemPickable pickable = hit.collider.GetComponent<ItemPickable>();
                if (pickable != null && inventory.Count < maxInventorySize)
                {
                    inventory.Add(pickable.itemData); // Agregar al inventario
                    Destroy(pickable.gameObject);
                    UpdateHotbarUI();
                }
            }
            
        }
    }

    private void HandleThrow()
    {
        if (Input.GetKeyDown(throwItemKey) && inventory.Count > 0)
        {
            ItemSO selectedItem = inventory[selectedIndex];
            if (itemPrefabs.TryGetValue(selectedItem.itemType, out GameObject prefab))
            {
                // Instanciar el ítem en la posición de la mano o jugador
                GameObject thrownItem = Instantiate(
                    prefab,
                    throwPosition.position,          // Posición inicial
                    throwPosition.rotation           // Rotación inicial (hacia donde mira el jugador)
                );

                thrownItem.transform.SetParent(null);
                // if (thrownItem.transform.childCount > 0)
                // {
                //     Transform child = thrownItem.transform.GetChild(0); // Obtener el primer hijo, puedes modificar esto según lo necesites
                //     Destroy(child.gameObject);  // Eliminar el hijo pero no el objeto principal
                // }
                
                // Añadir fuerza hacia adelante si el prefab tiene un Rigidbody
                Rigidbody rb = thrownItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    float throwForce = 10f; // Ajusta la magnitud de la fuerza según sea necesario
                    rb.AddForce(throwPosition.forward * throwForce, ForceMode.Impulse);
                }
    
                inventory.RemoveAt(selectedIndex);
                selectedIndex = FindNextValidIndex(selectedIndex);
                UpdateHotbarUI();
            }
        }
    }

    private void HandleSelection()
    {
        // Cambiar selección con teclas 1-4
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (i < inventory.Count) // Solo selecciona si hay un objeto en esa ranura
                {
                    selectedIndex = i;
                    UpdateHotbarUI();
                }
            }
        }

        // Cambiar selección con scroll, limitado entre 0 y maxInventorySize - 1
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && inventory.Count > 0)
        {
            if (scroll > 0 && selectedIndex < maxInventorySize - 1)
            {
                // Avanzar al siguiente índice si no estamos en el último
                selectedIndex++;
            }
            else if (scroll < 0 && selectedIndex > 0)
            {
                // Retroceder al índice anterior si no estamos en el primero
                selectedIndex--;
            }

            UpdateHotbarUI();
        }
    }


    public void UpdateHotbarUI()
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (i < inventory.Count)
            {
                hotbarSlots[i].sprite = inventory[i].itemSprite;
                hotbarSlots[i].enabled = true;
            }
            else
            {
                hotbarSlots[i].sprite = emptySlotSprite;
                hotbarSlots[i].enabled = true;
            }

            slotBackgrounds[i].color = i == selectedIndex ? selectedColor : defaultColor;
        }
    }


    private int FindNextValidIndex(int startIndex)
    {
        if (inventory.Count == 0) return 0; // Si no hay objetos, devolver 0

        int index = (startIndex + maxInventorySize) % maxInventorySize; // Ajustar índice en rango

        // Caso especial: si se elimina el último elemento, empezar desde el principio
        if (index >= inventory.Count)
        {
            index = 0;
        }

        while (index >= inventory.Count) // Saltar ranuras vacías
        {
            index = (index + 1) % maxInventorySize;
        }

        return index;
    }
}