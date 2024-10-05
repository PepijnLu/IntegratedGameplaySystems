using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    private void InstantiateUISlot(R_UIManager r_UIManager)
    {
        // Image slotIcon;
        GameObject slotPrefab = Resources.Load<GameObject>(slotPrefabPath);
        
        // Instantiate the loaded prefab
        GameObject slot = GameObject.Instantiate(slotPrefab);
        r_UIManager.UI_slotList.Add(slot);

        // Get image component
        Image slotImage = slot.GetComponent<Image>();
        slot.transform.SetParent(r_UIManager.UI_weaponInventory.transform);
        slot.name = t.Name;

        // Assign the sprite from the Iidentifiable to the sprite of the slot image
        slotImage.sprite = t.Icon;


        if(!r_UIManager.UI_slotDictionary.ContainsKey(slot.name)) 
        {
            // Add it to the dictionary of UI Slots
            r_UIManager.UI_slotDictionary.Add(slot.name, slot);
            
            // Create a new entry to store the name and the gameobject
            var entry = new SerializableDictionary<string, GameObject> 
            {   
                key = slot.name,
                value = slot
            };

            if(!r_UIManager.slotDictionaryEntry.Contains(entry)) 
            {
                r_UIManager.slotDictionaryEntry.Add(entry);
            }
        }
        else 
        {
            GameObject.Destroy(slot);
        }
    }
    
    private void RemoveFromInventoryUI() 
    {
        if (r_UIManager.UI_slotList == null || r_UIManager.UI_slotList.Count == 0) return;

        // Iterate through the list of weapon slots in the UI
        for (int i = 0; i < r_UIManager.UI_slotList.Count; i++)
        {
            var weaponSlot = r_UIManager.UI_slotList[i];

            if (weaponSlot == null)
            {
                Debug.LogWarning("Weapon slot is null, skipping...");
                continue;
            }

            Transform buttonPanel = weaponSlot.transform.GetChild(0);
            Transform removeButton = buttonPanel.GetChild(0);

            if(removeButton.gameObject.TryGetComponent<Button>(out var btn)) 
            {
                // Debug.Log($"Adding RemoveEvent listener to button: {removeButton.name}");
                btn.onClick.AddListener(() => RemoveEvent());
            }
            else
            {
                Debug.LogWarning($"No Button component found in the child of: {weaponSlot.name}");
            }
        }
    }

    private void RemoveEvent() 
    {
        if (r_WeaponsManager == null || r_WeaponsManager.weaponInventoryDictionary == null) return;
    
        // Iterate through the dictionary of weapon inventory using a for loop
        for (int i = 0; i < r_WeaponsManager.weaponInventoryDictionary.Count; i++)
        {
            var item = r_WeaponsManager.weaponInventoryDictionary.ElementAt(i);
            string key = item.Key;
            R_Weapon w = item.Value;

            // Check if the weapon is selected
            if (w.isSelectedInInventory)
            {
                // Check if the selected weapon matches the one in the UI slot
                if (r_UIManager.selectedSlot != null && r_UIManager.selectedSlot.Count > 0 && r_UIManager.selectedSlot[0].name == key)
                {
                    for (int j = 0; j < r_WeaponsManager.weaponsInventoryAsGameObjectList.Count; j++)
                    {
                        var w2 = r_WeaponsManager.weaponsInventoryAsGameObjectList[j];

                        if (key == w2.name)
                        {
                            // Mark the weapon as deselected
                            w.isSelectedInInventory = false;

                            // Remove the weapon from the inventory and dictionary
                            r_WeaponsManager.weaponsInventoryAsGameObjectList.Remove(w2);
                            r_WeaponsManager.weaponInventoryDictionary.Remove(key);
                            // r_WeaponsManager.inventoryEntries.RemoveAt(j);

                            // Update UI - remove slot from the UI
                            GameObject slot = r_UIManager.UI_slotList[j];
                            r_UIManager.UI_slotList.Remove(slot);

                            // Remove from entry point
                            r_UIManager.slotDictionaryEntry.RemoveAt(j);

                            // Remove from selected slot
                            r_UIManager.selectedSlot.Clear();

                            // Destroy the slot object
                            GameObject.Destroy(slot);

                            break; // Break out of the loop once the weapon is removed
                        }
                    }

                    break;
                }
                else
                {
                    Debug.Log($"Selected slot doesn't match the key or selectedSlot is null. Selected slot: {r_UIManager?.selectedSlot?[0]?.name}, Key: {key}");
                }

                break;
            }
            else
            {
                Debug.Log($"Weapon {key} is not selected.");
            }
        }
    }
}
