using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class R_UIManager
{
    [SerializeField] private GameObject UI_Inventory, slotPrefab;
    [SerializeField] List<GameObject> UI_Slots = new();
    // [SerializeField] string slotPrefabPath = "IntegratedGameplaySystems/Assets/Renato/Prefab/UI/InventorySlot";

    public void InstantiateUISlot(R_Weapon w) 
    {
        Image slotIcon;
        // GameObject slotPrefab = Resources.Load<GameObject>(slotPrefabPath);
        
        // Instantiate the loaded prefab
        GameObject slot = GameObject.Instantiate(slotPrefab);

        // Set parent to UI Inventory
        slot.transform.SetParent(UI_Inventory.transform, false);
        
        // Add an Image component if it doesn't exist
        if(slot.TryGetComponent<Image>(out var _slotIcon)) 
        {
            slotIcon = _slotIcon;
            slotIcon.sprite = w.icon;
        }
        else 
        {
            slotIcon = slot.AddComponent<Image>();
            slotIcon.sprite = w.icon;
        }
        
        UI_Slots.Add(slotIcon.gameObject);
    }
}
