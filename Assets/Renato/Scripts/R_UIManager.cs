using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class R_UIManager
{
    [SerializeField] public GameObject UI_Inv; // UI inventory
    [SerializeField] public List<SerializableDictionary<GameObject>> UISlotDictionaryEntries = new(); // Class entry to store the values from the dictionary
    [SerializeField] public List<GameObject> UI_SlotsList = new(); // List to store UI slots in (need it in order to add it into the dictionary)
    [SerializeField] private List<GameObject> usableWeaponUI = new();
    [SerializeField] private List<R_Weapon> selectedSlot = new();
    [SerializeField] private bool isPanelBeingUpdated = false;

    [HideInInspector] public GameObject slotPrefab; // This is just a placeholder for the slotPrefabPath
    [HideInInspector] public string slotPrefabPath = "InventorySlot"; // Path to the UI slot to instantiate
    public Dictionary<string, GameObject> UI_SlotsDict = new(); // Dictionary to store the UI slots

    public void Select(Dictionary<string, R_Weapon> weaponInvDict, WeaponGameManager weaponGameManager)  
    {
        // Iterate through the UI dictionary
        foreach (var slot in UI_SlotsDict)
        {
            // Assign the key and value from dictionary
            string slotName = slot.Key;
            GameObject slotObject = slot.Value;

            // Get the button component
            Button slotButton = slotObject.GetComponent<Button>();
            
            // Remove any previous listeners to avoid duplicate actions
            slotButton.onClick.RemoveAllListeners();

            // Add a new listener to handle the click event
            slotButton.onClick.AddListener(() => OnSlotClicked(slotName, weaponInvDict, weaponGameManager));   
        }
    }

    private void OnSlotClicked(string slotName, Dictionary<string, R_Weapon> dictionary, WeaponGameManager weaponGameManager) 
    {
        Debug.Log($"Clicked on slot: {slotName}");

        // Check if the slot exists in the weapon inventory dictionary
        if (dictionary.TryGetValue(slotName, out R_Weapon w)) 
        {
            // Get the previously selected weapon if there is one, otherwise set it to null
            R_Weapon previousW = selectedSlot.Count > 0 ? selectedSlot[0] : null;

            // If no weapon is selected or if it's a new weapon
            if (!selectedSlot.Contains(w)) 
            {
                // If a weapon was previously selected, deselect it
                if (previousW != null) 
                {
                    previousW.selected = false;
                    selectedSlot.Remove(previousW);  // Remove previous weapon from selection
                    Debug.Log($"Deselected weapon: {previousW.Name}");
                }

                // Add the new weapon to the selected slot and mark it as selected
                selectedSlot.Add(w);
                OpenSelectPanel(weaponGameManager);
                w.selected = true;
                Debug.Log($"Selected weapon: {w.Name}");
            }
            else
            {
                // If the weapon is already selected, deselect it
                w.selected = false;
                selectedSlot.Remove(w);
                Debug.Log($"Deselected weapon: {w.Name}");
            }
        }
        else 
        {
            Debug.LogWarning($"No weapon found for slot: {slotName}");
        }
    }

    private void OpenSelectPanel(WeaponGameManager weaponGameManager) 
    {
        if (isPanelBeingUpdated) return; // Prevent re-entry
        
        isPanelBeingUpdated = true;
        foreach (var w in selectedSlot)
        {
            if(w.selected) 
            {
                foreach (var slot in UI_SlotsList)
                {
                    if (slot == null)
                    {
                        Debug.LogWarning($"Slot {slot.name} is null or not initialized properly");
                        return;
                    }

                    if(w.Name == slot.name) 
                    {
                        Transform selectPanelTransform = slot.transform.Find("Select Panel");

                        if(selectPanelTransform == null) 
                        {
                            Debug.LogWarning("No select panel couldnt be found!");
                            return;
                        }

                        GameObject selectPanel = selectPanelTransform.gameObject;
                        Debug.Log($"{selectPanel.name} has been found under slot: {slot.name}");

                        if (selectPanel.activeSelf)
                        {
                            Debug.Log($"{selectPanel.name} is already active.");
                        }
                        else
                        {
                            selectPanel.SetActive(true);
                            Debug.Log($"{selectPanel.name} is now set to active.");
                        }

                        if(!slot.TryGetComponent<Button>(out var btn)) 
                        {
                            Debug.Log($"No button to be found on the {slotPrefab.name}");
                            return;
                        }

                        Debug.Log($"{btn.name} is not null");
                        btn.enabled = false;

                        // Add event listener to the button of the selectPanel
                        if(!selectPanel.TryGetComponent<Button>(out var btn2)) 
                        {
                            Debug.LogWarning($"No button to be found on the {selectPanel.name}");
                            return;
                        } 

                        Debug.Log($"{btn2.name} on the slot has been found");

                        btn2.onClick.RemoveAllListeners();
                        btn2.onClick.AddListener(() => OnAddClick(weaponGameManager));
                    }
                    else 
                    {
                        Debug.LogWarning("Names are not the same");
                    }
                }
            }
            else 
            {
                return;
            }

        }

        isPanelBeingUpdated = false;
    }

    private void OnAddClick(WeaponGameManager weaponGameManager) 
    {
        // Add the weapon from the weapon inventory list to the list of usable weapons
        R_Weapon w = selectedSlot[0];

        // Add to the list and dictionary
        if(!weaponGameManager.usableWeapons.Contains(w) && weaponGameManager.usableWeapons.Count < 2) 
        {
            weaponGameManager.usableWeapons.Add(w);
            weaponGameManager.usableWeapnDict.Add(w.Name, w); 
        }

        // Create a new entry point for the dictionary
        var entry = new SerializableDictionary<R_Weapon> 
        {   
            key = w.Name,
            value = w
        };

        // Add it in the list of usable entries
        weaponGameManager.usableEntries.Add(entry);

        // Iterate through the UI slot list
        for (int i = 0; i < UI_SlotsList.Count; i++)
        {
            GameObject slot = UI_SlotsList[i];
            if(slot.name == w.Name) 
            {
                if(usableWeaponUI.Count < 2 && !usableWeaponUI.Contains(slot)) 
                {
                    usableWeaponUI.Add(slot);
                }
            }
        }

        // Remove it from the weapon inventory list of game objects
        for (int i = 0; i < weaponGameManager.weaponInventory.Count; i++)
        {
            GameObject _w = weaponGameManager.weaponInventory[i];
            if(_w.name == w.Name) 
            {
                weaponGameManager.weaponInventory.Remove(_w);
            }
        }

        // Remove it from the inventory dictionary
        if(weaponGameManager.weaponInvDict.ContainsKey(w.Name)) 
        {
            weaponGameManager.weaponInvDict.Remove(w.Name);
        }

        // Remove it from the weapon inventory entry
        for (int i = 0; i < weaponGameManager.inventoryEntries.Count; i++)
        {
            if(weaponGameManager.inventoryEntries[i].key == w.Name) 
            {
                weaponGameManager.inventoryEntries.Remove(weaponGameManager.inventoryEntries[i]);
            }
        }

        // Remove it from the UI slot dictionary
        for (int i = 0; i < UI_SlotsList.Count; i++)
        {
            GameObject slot = UI_SlotsList[i];
            if(slot.name == w.Name) 
            {
                UI_SlotsList.Remove(slot);
            }
        }

        // Remove the UI slot from the dictionary
        if(UI_SlotsDict.ContainsKey(w.Name)) 
        {
            UI_SlotsDict.Remove(w.Name);
        }

        // Remove it from the UI slot dicrionary entry
        for (int i = 0; i < UISlotDictionaryEntries.Count; i++)
        {
            if(UISlotDictionaryEntries[i].key == w.Name) 
            {
                UISlotDictionaryEntries.Remove(UISlotDictionaryEntries[i]);
            }
        }

        w.selected = false;
        selectedSlot.Remove(w);

        // Destroy the slot or move it under a new game object of UI
    }
}