using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")] 
    public List<itemType> inventoryList;
    public int selectedItem = 0;
    public float playerReach = 6;
    [SerializeField] GameObject throwItem_GameObject;
    
    [Space(4)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;
    
    [Space(4)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject woodlog_item;
    [SerializeField] GameObject medkit_item;
    
    [Space(4)]
    [Header("Item prefabs")]
    [SerializeField] GameObject woodlog_prefab;
    [SerializeField] GameObject medkit_prefab;

    [Space(4)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[4];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[4];
    [SerializeField] Sprite emptySlotSprite;
    

    [SerializeField] Camera cam;
    [SerializeField] GameObject pickUp_gameObject;
    
    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, int> inventory = new Dictionary<itemType, int>();


    void Start()
    {
        // Asociamos tipos de objetos con sus GameObjects
        itemSetActive.Add(itemType.WoodLog, woodlog_item);
        itemSetActive.Add(itemType.Medkit, medkit_item);
        
        itemInstantiate.Add(itemType.WoodLog, woodlog_prefab);
        itemInstantiate.Add(itemType.Medkit, medkit_prefab);
        
        selectedItem = 0;
        UpdateHotbarUI();
        NewItemSelected();
    }

    void Update()
    {
        //Items pickup
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, playerReach))
        {
            Debug.Log($"Impacto con: {hitInfo.collider.name}"); // Verifica qué objeto impacta
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                pickUp_gameObject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    if (inventoryList.Count < inventorySlotImage.Length)
                    {
                        inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                        item.PickItem();
                        UpdateHotbarUI();
                    }
                    else
                    {
                        Debug.Log("Inventario lleno");
                    }
                }
            }
            else
            {
                pickUp_gameObject.SetActive(false);
            }
        }
        else
        {
            pickUp_gameObject.SetActive(false);
        }

        //Items throw
        if (Input.GetKey(throwItemKey) && inventoryList.Count > 1)
        {
            // Obtén el tipo de objeto a lanzar
            var itemToDrop = inventoryList[selectedItem];

            // Instanciar el objeto lanzado
            Instantiate(itemInstantiate[itemToDrop], throwItem_GameObject.transform.position, Quaternion.identity);

            // Eliminar el objeto del inventario
            inventoryList.RemoveAt(selectedItem);

            // Ajustar el índice seleccionado si el último objeto fue eliminado
            if (selectedItem >= inventoryList.Count)
                selectedItem = Mathf.Max(0, inventoryList.Count - 1);

            UpdateHotbarUI();
        }


        

        

        int a = 0;

        foreach (Image image in inventoryBackgroundImage)
        {
            if (a == selectedItem)
            {
                image.color = new Color32 (145, 255, 126, 255);
            }
            else
            {
                image.color = new Color32 (255, 255, 255, 255);
            }

            a++;
        }
        
        HandleMouseWheel();
        HandleHotkeys();
    }
    
    //UI
    
    private void UpdateHotbarUI()
    {
        int slotsToUpdate = Mathf.Min(inventorySlotImage.Length, inventoryList.Count);

        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            if (i < slotsToUpdate)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.item_sprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }

            // Actualizar el color de la cuadrícula según la selección
            inventoryBackgroundImage[i].color = (i == selectedItem)
                ? new Color32(145, 255, 126, 255) // Verde
                : new Color32(255, 255, 255, 255); // Blanco
        }
    }




    private void HandleMouseWheel()
    {
        if (inventoryList.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            selectedItem = (selectedItem + 1) % inventoryList.Count; // Ciclar hacia adelante
            NewItemSelected();
        }
        else if (scroll < 0f)
        {
            selectedItem = (selectedItem - 1 + inventoryList.Count) % inventoryList.Count; // Ciclar hacia atrás
            NewItemSelected();
        }
    }

    private void HandleHotkeys()
    {
        if (inventoryList.Count == 0) return;

        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Detecta las teclas 1, 2, 3, ..., 9
            {
                selectedItem = i;
                NewItemSelected();
                break;
            }
        }
    }

    private void NewItemSelected()
    {
        // Desactiva todos los objetos en `itemSetActive`
        foreach (var item in itemSetActive.Values)
        {
            if (item != null)
                item.SetActive(false); // Asegúrate de no activar objetos nulos
        }

        // Verifica que hay elementos en el inventario antes de activar
        if (inventoryList.Count > 0 && selectedItem < inventoryList.Count)
        {
            var selectedItemType = inventoryList[selectedItem];
            if (itemSetActive.TryGetValue(selectedItemType, out var selectedItemGameObject) && selectedItemGameObject != null)
            {
                selectedItemGameObject.SetActive(true);
            }
        }
    }


}

public interface IPickable
{
    void PickItem();
}
