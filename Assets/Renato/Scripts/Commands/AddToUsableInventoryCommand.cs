using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToUsableInventoryCommand: ICommand
{
    private readonly Player player;
    private readonly R_WeaponsManager weaponGameManager;                                
    private readonly R_UIManager UIManager;
    private readonly List<R_Weapon> selectedSlot;

    public AddToUsableInventoryCommand
    (
        Player player,
        List<R_Weapon> selectedSlot,
        R_WeaponsManager weaponGameManager,
        R_UIManager UIManager
    ) 
    {
        this.player = player;
        this.selectedSlot = selectedSlot;
        this.weaponGameManager = weaponGameManager;
        this.UIManager = UIManager;
    }

    public void Execute()
    {
        Select(UIManager);
    }

    public void Undo()
    {
        
    }

    private void Select(R_UIManager r_UIManager)  
    {
        // Iterate through the UI dictionary
        foreach (var slot in r_UIManager.UI_slotDictionary)
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
                    slotButton.onClick.AddListener(() => OnSlotClicked(slotName, weaponGameManager.weaponInventoryDictionary, r_UIManager));   
                }
            }
        }
    }

    public void OnSlotClicked(string slotName, Dictionary<string, R_Weapon> dictionary, R_UIManager r_UIManager) 
    {
        Debug.Log($"Clicked on slot: {slotName}");  // Add this line

        // Check if the slot exists in the weapon inventory dictionary
        if (dictionary.TryGetValue(slotName, out R_Weapon w)) 
        {
            Debug.Log($"Weapon found in inventory for slot: {slotName}, Weapon Name: {w.Name}");  // Add this log

            // Get the previously selected weapon if there is one, otherwise set it to null
            R_Weapon previousW = selectedSlot.Count > 0 ? selectedSlot[0] : null;

            if (previousW != null) 
            {
                Debug.Log($"Previous weapon selected: {previousW.Name}");  // Add this log
            }

            // If no weapon is selected or if it's a new weapon
            if (!selectedSlot.Contains(w)) 
            {
                Debug.Log($"Selecting weapon: {w.Name}");  // Log the selection

                // If a weapon was previously selected, deselect it
                if (previousW != null) 
                {
                    previousW.isSelectedInInventory = false;
                    selectedSlot.Remove(previousW);  // Remove previous weapon from selection
                    ActivateButtonPanel(previousW.Name, false, r_UIManager);
                }

                // Add the new weapon to the selected slot and mark it as selected
                selectedSlot.Add(w);
                w.isSelectedInInventory = true;
                ActivateButtonPanel(slotName, true, r_UIManager);
            }
            else
            {
                Debug.Log($"Deselecting weapon: {w.Name}");  // Log the deselection

                // If the weapon is already selected, deselect it
                w.isSelectedInInventory = false;
                selectedSlot.Remove(w);
                ActivateButtonPanel(slotName, false, r_UIManager);
            }
        }
        else 
        {
            Debug.LogWarning($"No weapon found for slot: {slotName}");  // Add a warning if no weapon is found
        }
    }

    private void ActivateButtonPanel(string slotName, bool isActive, R_UIManager r_UIManager)
    {
        // Find the slot in the slot list UI
        foreach (var slot in r_UIManager.UI_slotList)
        {
            if (slot.name == slotName)
            {
                // Fetch the button panel
                GameObject buttonPanel = slot.transform.GetChild(0).gameObject;
                buttonPanel.SetActive(isActive);

                if(isActive) 
                {
                    // Fetch the add and remove button
                    GameObject removeButton = buttonPanel.transform.GetChild(0).gameObject;
                    GameObject addButton = buttonPanel.transform.GetChild(1).gameObject;
                    removeButton.SetActive(isActive);
                    addButton.SetActive(isActive);

                    if(!addButton.TryGetComponent<Button>(out var addBtn)) 
                    {
                        Debug.LogWarning("No add button to be found!");
                        return;
                    }

                    addBtn.onClick.AddListener(() => OnClickAdd(ref addButton, ref removeButton));
                }
            }
        }
    }

    private void OnClickAdd(ref GameObject addBtn, ref GameObject removeBtn) 
    {
        if (selectedSlot.Count == 1) 
        {
            R_Weapon w = selectedSlot[0];

            if (w == null) 
            {
                Debug.LogError("Selected weapon is null!");  // Add this line
                return; 
            }

            Debug.Log($"Trying to add weapon: {w.Name} to usable inventory");  // Add this log

            if (!weaponGameManager.usableWeaponDictionary.ContainsValue(w) && weaponGameManager.usableWeaponDictionary.Count < 2) 
            {
                weaponGameManager.usableWeaponDictionary.Add(w.Name, w);

                // Log the dictionary addition
                Debug.Log($"Added weapon: {w.Name} to usableWeaponDictionary");

                // Create a new entry point for the dictionary
                var entry = new SerializableDictionary<string, R_Weapon> 
                {   
                    key = w.Name,
                    value = w
                };

                // Add it to the list of usable entries
                weaponGameManager.usableWeaponInventoryEntries.Add(entry);

                Debug.Log($"Added {w.Name} to usableWeaponInventoryEntries list");  // Log this

                // Update UI (iterate in reverse to avoid index errors)
                for (int i = UIManager.UI_slotList.Count - 1; i >= 0; i--)
                {
                    GameObject slot = UIManager.UI_slotList[i];

                    if (slot == null) 
                    {
                        Debug.LogError($"Slot at index {i} is null!");  // Add this log
                        continue;
                    }

                    if (string.Equals(slot.name.Trim(), w.Name.Trim(), System.StringComparison.OrdinalIgnoreCase))
                    {
                        UIManager.UI_UsableWeaponsList.Add(slot);
                        UIManager.UI_slotList.RemoveAt(i); // Remove from the inventory list

                        Debug.Log($"Moved {w.Name} to UI_UsableWeaponsList");  // Log this

                        // Move the slot to the usable inventory panel
                        slot.transform.SetParent(UIManager.UI_usableWeaponsInventory.transform);

                        break; 
                    }
                }

                // Entry point for the UI slots (iterate in reverse to avoid index issues)
                for (int i = UIManager.slotDictionaryEntry.Count - 1; i >= 0; i--)
                {
                    var item = UIManager.slotDictionaryEntry[i];
                    if (item.key == w.Name) 
                    {
                        UIManager.UI_slotDictionary.Remove(item.key, out item.value);
                        UIManager.slotDictionaryEntry.RemoveAt(i);

                        Debug.Log($"Removed {w.Name} from slotDictionaryEntry");  // Log this

                        break;
                    }
                }

                // Inventory of weapon game objects (iterate in reverse)
                for (int i = weaponGameManager.weaponsInventoryAsGameObjectList.Count - 1; i >= 0; i--)
                {
                    var weapon = weaponGameManager.weaponsInventoryAsGameObjectList[i];
                    if (weapon.name == w.Name) 
                    {
                        weaponGameManager.usableWeaponsAsGameObjectList.Add(weapon);
                        weapon.transform.SetParent(player.usableInventory.transform);

                        Debug.Log($"Moved weapon GameObject {w.Name} to usable inventory");  // Log this

                        break;
                    }
                }

                if (weaponGameManager.usableWeaponDictionary.Count <= 2) 
                {
                    DeactivateSelectPanels(UIManager);
                    addBtn.SetActive(false);
                    removeBtn.SetActive(false);
                }
            } 

            w.isSelectedInInventory = false;
            selectedSlot.Clear(); 
        }
    }

    private void DeactivateSelectPanels(R_UIManager r_UIManager)
    {
        foreach (GameObject item in r_UIManager.UI_UsableWeaponsList)
        {
            Transform selectPanelTransform = item.transform.Find("ButtonPanel");
            if (selectPanelTransform != null)
            {
                selectPanelTransform.gameObject.SetActive(false);
                // Debug.Log($"Deactivated Select Panel for: {item.name}");

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