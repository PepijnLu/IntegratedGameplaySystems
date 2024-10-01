using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class R_UIManager
{
    [SerializeField] private GameObject UI_Inventory, slotPrefab;
    [SerializeField] List<GameObject> UI_Slots = new();

    public void InstantiateUISlot(R_Weapon w) 
    {
        // Get the scriptable object icon
        // // The scriptable object should have a relation with the weapon
        // Image icon = w.icon;

        // // Instantiate a new game object
        // GameObject slot = Instantiate(slotPrefab);
        // // Transform the new game object under the UI inventory
        // slot.transform.SetParent(UI_Inventory.transform);
        // // Add the image component
        // Image slotIcon = slot.AddComponent<Image>();
        // // Assign the weapons icon to the image slot
        // slotIcon.sprite = 
        
    }
}
