using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddToInventoryCommandUI<T> : ICommand where T : IIdentifiable
{
    private readonly R_UIManager r_UIManager;
    private readonly R_WeaponsManager r_WeaponsManager;
    readonly private string slotPrefabPath = "";
    readonly private T t;

    public AddToInventoryCommandUI
    (
        R_WeaponsManager r_WeaponsManager,
        R_UIManager r_UIManager,
        string slotPrefabPath,
        T t
    ) 
    {
        this.r_WeaponsManager = r_WeaponsManager;
        this.r_UIManager = r_UIManager;
        this.slotPrefabPath = slotPrefabPath;
        this.t = t;
    } 

    public void Execute()
    {
        // Add to inventory UI
        InstantiateUISlot(r_UIManager);
    }

    public void Undo()
    {
        if (r_UIManager == null)
        {
            Debug.LogError("R_UIManager is null! Cannot undo the command.");
            return;
        }

        // Remove from inventory UI
        RemoveFromInventoryUI();
    }

    private void InstantiateUISlot(R_UIManager UIManager)
    {
        // Load the slot prefab
        GameObject slotPrefab = Resources.Load<GameObject>(slotPrefabPath);
        
        // Instantiate the loaded prefab
        GameObject slot = GameObject.Instantiate(slotPrefab);
        UIManager.UI_slotList.Add(slot);

        // Get the image component of the slot and assign the sprite
        Image slotImage = slot.GetComponent<Image>();
        slot.transform.SetParent(UIManager.UI_weaponInventory.transform);
        slot.name = t.Name;
        slotImage.sprite = t.Icon;

        // If the slot doesn't already exist in the dictionary
        if (!UIManager.UI_slotDictionary.ContainsKey(slot.name)) 
        {
            // Add it to the dictionary of UI Slots
            UIManager.UI_slotDictionary.Add(slot.name, slot);
            
            // Create a new entry to store the name and the gameobject
            var entry = new SerializableDictionary<string, GameObject> 
            {   
                key = slot.name,
                value = slot
            };

            if (!UIManager.slotDictionaryEntry.Contains(entry)) 
            {
                UIManager.slotDictionaryEntry.Add(entry);
            }
        }
        else 
        {
            GameObject.Destroy(slot); // Slot already exists, destroy the duplicate
        }
    }

    private void RemoveFromInventoryUI() 
    {
        if (r_UIManager.UI_slotList == null || r_UIManager.UI_slotList.Count == 0) return;

        foreach (var weaponSlot in r_UIManager.UI_slotList)
        {
            if (weaponSlot == null)
            {
                Debug.LogWarning("Weapon slot is null, skipping...");
                continue;
            }

            Transform buttonPanel = weaponSlot.transform.GetChild(0);
            Transform removeButton = buttonPanel.GetChild(0);

            if(removeButton.gameObject.TryGetComponent<Button>(out var btn)) 
            {
                // Instead of adding a listener each time, check if it exists
                if (!btn.onClick.GetPersistentEventCount().Equals(0)) continue;

                // Cache the current weapon name to pass to the RemoveEvent method
                string weaponName = weaponSlot.name;
                btn.onClick.AddListener(() => RemoveEvent(weaponName));
            }
            else
            {
                Debug.LogWarning($"No Button component found in the child of: {weaponSlot.name}");
            }
        }
    }

    private void RemoveEvent(string weaponName) 
    {
        if (r_WeaponsManager == null || r_WeaponsManager.weaponInventoryDictionary == null) return;

        // If the weapon exists in the dictionary, process it
        if (r_WeaponsManager.weaponInventoryDictionary.TryGetValue(weaponName, out var weapon))
        {
            if (!weapon.isSelectedInInventory)
            {
                Debug.Log($"Weapon {weaponName} is not selected.");
                return;
            }

            // Deselect the weapon and remove it from the dictionary
            weapon.isSelectedInInventory = false;
            r_WeaponsManager.weaponInventoryDictionary.Remove(weaponName);

            // Find and remove the slot from the UI
            for (int i = 0; i < r_UIManager.UI_slotList.Count; i++)
            {
                GameObject slot = r_UIManager.UI_slotList[i];
                if (slot.name == weaponName)
                {
                    // Remove from slot list and destroy the slot
                    r_UIManager.UI_slotList.Remove(slot);
                    r_UIManager.selectedSlot.Clear(); // Clear selected slots if needed

                    GameObject.Destroy(slot); // Destroy the slot GameObject
                    Debug.Log($"Weapon {weaponName} successfully removed.");
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning($"Weapon {weaponName} not found in inventory dictionary.");
        }
    }
}
