using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWeaponCommand: ICommand
{
    private readonly R_WeaponsManager weaponGameManager;                                
    private readonly R_UIManager r_UIManager;
    private readonly List<R_Weapon> selectedSlot;                                   

    public SelectWeaponCommand
    (
        List<R_Weapon> selectedSlot,
        R_WeaponsManager weaponGameManager,
        R_UIManager r_UIManager
    ) 
    {
        this.selectedSlot = selectedSlot;
        this.weaponGameManager = weaponGameManager;
        this.r_UIManager = r_UIManager;
    }

    public void Execute()
    {
        Select(r_UIManager);
    }

    public void Undo()
    {
        
    }

    public void Select(R_UIManager r_UIManager)  
    {
        // Iterate through the UI dictionary
        foreach (var slot in r_UIManager.slotDictionaryUI)
        {
            // Assign the key and value from dictionary
            string slotName = slot.Key;
            GameObject slotObject = slot.Value;

            if(slotObject != null) 
            {
                // Get the button component
                if(slotObject.TryGetComponent<Button>(out var slotButton)) 
                {
                    // Remove any previous listeners to avoid duplicate actions
                    slotButton.onClick.RemoveAllListeners();

                    // Add a new listener to handle the click event
                    slotButton.onClick.AddListener(() => OnSlotClicked(slotName, weaponGameManager.weaponInvDictionary, r_UIManager));   
                }
            }
        }
    }

    private void OnSlotClicked(string slotName, Dictionary<string, R_Weapon> dictionary, R_UIManager r_UIManager) 
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
                    SetSelectPanelActive(previousW.Name, false, r_UIManager);
                }

                // Add the new weapon to the selected slot and mark it as selected
                selectedSlot.Add(w);
                w.selected = true;
                SetSelectPanelActive(slotName, true, r_UIManager);
            }
            else
            {
                // If the weapon is already selected, deselect it
                w.selected = false;
                selectedSlot.Remove(w);
                SetSelectPanelActive(slotName, false, r_UIManager);
            }
        }
        else 
        {
            Debug.LogWarning($"No weapon found for slot: {slotName}");
        }
    }

    private void SetSelectPanelActive(string slotName, bool isActive, R_UIManager r_UIManager)
    {
        // Find the slot in the slot list UI
        foreach (var slot in r_UIManager.slotListUI)
        {
            if (slot.name == slotName)
            {
                // Find the "ButtonPanel" child object
                Transform buttonPanelTransform = slot.transform.Find("ButtonPanel");
                if (buttonPanelTransform != null)
                {
                    GameObject buttonPanel = buttonPanelTransform.gameObject;

                    // Set the active state of the select panel
                    buttonPanel.SetActive(isActive);
                    if(isActive == true) 
                    {
                        Transform specificChild = buttonPanel.transform.GetChild(1);
                        
                        if(specificChild.TryGetComponent<Button>(out var btn)) 
                        {
                            btn.onClick.AddListener(() => OnAddClick(weaponGameManager, r_UIManager));
                        }
                    }
                }
            }
        }
    }

    private void OnAddClick(R_WeaponsManager weaponGameManager, R_UIManager r_UIManager) 
    {
        R_Weapon w = selectedSlot[0];
        if(w == null) return; 

        // Add to the list and dictionary
        if (!weaponGameManager.usableWeapons.Contains(w) && weaponGameManager.usableWeapons.Count < 2) 
        {
            weaponGameManager.usableWeapons.Add(w);
            weaponGameManager.usableWeapnDictionary.Add(w.Name, w); 
               
            // Create a new entry point for the dictionary
            var entry = new SerializableDictionary<R_Weapon> 
            {   
                key = w.Name,
                value = w
            };

            // Add it in the list of usable entries
            weaponGameManager.usableEntries.Add(entry);

            // Add the weapon to the usable weapon UI
            for (int i = 0; i < r_UIManager.slotListUI.Count; i++)
            {
                GameObject slot = r_UIManager.slotListUI[i];
                if (string.Equals(slot.name.Trim(), w.Name.Trim(), System.StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"Match found for slot: {slot.name} and weapon: {w.Name}"); // Log this to ensure match happens

                    r_UIManager.usableWeaponsUI.Add(slot);
                    r_UIManager.slotListUI.Remove(slot); 

                    // Move the slot to the usable inventory panel
                    slot.transform.SetParent(r_UIManager.usableInventoryUI.transform);
                    break; 
                }
            }

            for (int i = 0; i < r_UIManager.slotDictionaryEntry.Count; i++)
            {
                var item = r_UIManager.slotDictionaryEntry[i];
                if(item.key == w.Name) 
                {
                    r_UIManager.slotDictionaryUI.Remove(item.key, out item.value);
                    r_UIManager.slotDictionaryEntry.Remove(item);
                    
                    break;
                }
            }

            for (int i = 0; i < weaponGameManager.weaponInventory.Count; i++)
            {
                var item = weaponGameManager.weaponInventory[i];
                if(item.name == w.Name) 
                {
                    weaponGameManager.weaponInventory.Remove(item);

                    break;
                }
            }

            for (int i = 0; i < weaponGameManager.inventoryEntries.Count; i++)
            {
                var item = weaponGameManager.inventoryEntries[i];
                if(item.key == w.Name) 
                {
                    weaponGameManager.weaponInvDictionary.Remove(item.key, out item.value);
                    weaponGameManager.inventoryEntries.Remove(item);
                }
            }

              
            // // Move the slot to the usable inventory on the canvas
            if(weaponGameManager.usableWeapons.Count <= 2) 
            {
                DeactivateSelectPanels(r_UIManager);
            }
        }
    }


    private void DeactivateSelectPanels(R_UIManager r_UIManager)
    {
        foreach (GameObject item in r_UIManager.usableWeaponsUI)
        {
            Debug.Log($"Processing UI item for deactivation: {item.name}");

            Transform selectPanelTransform = item.transform.Find("ButtonPanel");
            if (selectPanelTransform != null)
            {
                selectPanelTransform.gameObject.SetActive(false);
                Debug.Log($"Deactivated Select Panel for: {item.name}");

                // Remove listeners to avoid conflicts
                if (item.TryGetComponent<Button>(out var btn))
                {
                    btn.onClick.RemoveAllListeners();
                }
            }
            else 
            {
                Debug.LogWarning("Select Panel Not Found for item: " + item.name);
            }
        }
    }
}