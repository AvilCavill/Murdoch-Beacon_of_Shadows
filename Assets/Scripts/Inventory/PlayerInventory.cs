using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")] 
    public List<itemType> inventoryList;
    public int selectedItem = 0;
    public float playerReach;
    
    [Space(4)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;
    
    [Space(4)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject woodlog_item;
    [SerializeField] GameObject medkit_item;


    [SerializeField] private Camera cam;
    
    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };

    void Start()
    {
        // Asociamos tipos de objetos con sus GameObjects
        itemSetActive.Add(itemType.WoodLog, woodlog_item);
        itemSetActive.Add(itemType.Medkit, medkit_item);
        
        NewItemSelected();
    }

    void Update()
    {
        //Items pickup
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, playerReach) && Input.GetKey(pickItemKey))
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                //inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>());
            }
        }
        HandleMouseWheel();
        HandleHotkeys();
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
        // Desactiva todos los objetos
        foreach (var item in itemSetActive.Values)
        {
            item.SetActive(false);
        }
        
        // Activa solo el objeto seleccionado
        if (inventoryList.Count > 0)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            selectedItemGameObject.SetActive(true);
        }
    }
}

public interface IPickable
{
    void PickItem();
}
