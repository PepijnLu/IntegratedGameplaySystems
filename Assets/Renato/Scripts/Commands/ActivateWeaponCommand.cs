using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActivateWeaponCommand : ICommand
{
    private AddToUsableInventoryCommand selectCommand;
    private readonly R_WeaponsManager weaponManager;
    private readonly R_UIManager UIManager;
    private readonly Player player;

    public ActivateWeaponCommand
    (
        Player player,
        R_WeaponsManager weaponManager,
        R_UIManager UIManager
    ) 
    {
        this.player = player;
        this.weaponManager = weaponManager;
        this.UIManager = UIManager;
    }

    public void Execute()
    {
        SetupWeaponSelection();
    }

    public void Undo()
    {
    }

    private void SetupWeaponSelection() 
    {
        // Iterate through the UI list
        for (int i = 0; i < UIManager.UI_UsableWeaponsList.Count; i++)
        {
            GameObject UISlot = UIManager.UI_UsableWeaponsList[i];
            if (UISlot.TryGetComponent<Button>(out var button))
            {
                // Iterate through the usable weapon dictionary
                foreach (var element in weaponManager.usableWeaponDictionary)
                {
                    // Compare name
                    if(element.Key == UISlot.name) 
                    {
                        // Remove previous listeners to avoid duplicate events
                        button.onClick.RemoveAllListeners();
                        
                        // Only then add a listener to the button
                        button.onClick.AddListener(() => OnSlotClicked(element.Value));
                    }
                }
            }
        }
    }

    // UI slot selection
    private void OnSlotClicked(R_Weapon weapon)
    {
        // Toggle selection state
        weapon.isSelectedInUsable = !weapon.isSelectedInUsable;

        if (weapon.isSelectedInUsable)
        {
            // Deselect other weapons when a new one is selected
            DeselectAllWeaponsExcept(weapon);

            // Select and open the button panel for the clicked weapon
            UpdateButtonPanel(weapon, true);
        }
        else
        {
            // Deselect and close the button panel
            UpdateButtonPanel(weapon, false);
        }
    }

    private void DeselectAllWeaponsExcept(R_Weapon selectedWeapon)
    {
        foreach (var element in weaponManager.usableWeaponDictionary)
        {
            if (element.Value.isSelectedInUsable && element.Value != selectedWeapon)
            {
                element.Value.isSelectedInUsable = false;
                UpdateButtonPanel(element.Value, false);
            }
        }
    }

    // Button Panel
    private void UpdateButtonPanel(R_Weapon weapon, bool isActive)
    {
        foreach (var slot in UIManager.UI_UsableWeaponsList)
        {
            if (slot.name == weapon.Name)
            {
                GameObject buttonPanel = slot.transform.GetChild(0).gameObject;
                buttonPanel.SetActive(isActive);  // Show or hide the panel based on selection

                if (isActive)
                {
                    GameObject invBtnObj = buttonPanel.transform.GetChild(2).gameObject;
                    GameObject useBtnObj = buttonPanel.transform.GetChild(3).gameObject;
                    invBtnObj.SetActive(true);
                    useBtnObj.SetActive(true);

                    if (invBtnObj.TryGetComponent<Button>(out var toInventoryButton) &&
                        useBtnObj.TryGetComponent<Button>(out var equipWeaponButton))
                    {
                        // Add listeners for the buttons
                        toInventoryButton.onClick.AddListener(() => AddToInventory(weapon, ref buttonPanel, ref invBtnObj, ref useBtnObj));
                        equipWeaponButton.onClick.AddListener(() => ActivateWeapon(ref buttonPanel, ref invBtnObj, ref useBtnObj));
                    }
                    else
                    {
                        Debug.LogError("No buttons found!");
                    }
                }
            }
        }
    }

    // Activate weapon
    private void ActivateWeapon(ref GameObject buttonPanel, ref GameObject invBtnObj, ref GameObject useBtnObj)
    {        
        foreach (var element in weaponManager.usableWeaponsAsGameObjectList)
        {
            element.SetActive(false);  // Deactivate all weapon GameObjects
        }

        foreach (var element in weaponManager.usableWeaponDictionary)
        {
            element.Value.isActive = false; 

            if (element.Value.isSelectedInUsable) 
            {
                GameObject weaponGameObject = weaponManager.usableWeaponsAsGameObjectList.FirstOrDefault(wgo => wgo.name == element.Value.Name);

                if (weaponGameObject != null)
                {
                    weaponManager.activeWeapon = element.Value;
                    weaponManager.activeWeaponGameObject = weaponGameObject;
                    weaponGameObject.SetActive(true);
                    element.Value.isActive = true;

                    buttonPanel.SetActive(false);
                    invBtnObj.SetActive(false);
                    useBtnObj.SetActive(false);

                    Debug.Log($"Activated weapon: {element.Value.Name}");
                }
            }

        }
        
        // Reset selection state for all weapons
        foreach (var element in weaponManager.usableWeaponDictionary)
        {
            element.Value.isSelectedInUsable = false;
        }
    }

    private void AddToInventory(R_Weapon weapon, ref GameObject buttonPanel, ref GameObject inventoryButton, ref GameObject useButton)
    {
        weapon.isSelectedInUsable = false;

        for (int i = 0; i < UIManager.UI_UsableWeaponsList.Count; i++)
        {
            GameObject UiSlot = UIManager.UI_UsableWeaponsList[i];

            if (UiSlot.name == weapon.Name)
            {
                // Move the slot back to the inventory UI panel
                UIManager.UI_UsableWeaponsList.Remove(UiSlot);
                UiSlot.transform.SetParent(UIManager.UI_weaponInventory.transform);

                if (UiSlot.TryGetComponent<Button>(out var button))
                {
                    // Remove any old listeners attached to this button
                    button.onClick.RemoveAllListeners();

                    // Re-add the listener to allow re-adding the weapon to usable inventory
                    selectCommand = new AddToUsableInventoryCommand(
                        player,
                        UIManager.selectedSlot,
                        weaponManager,
                        UIManager
                    );

                    button.onClick.AddListener(() => selectCommand.OnSlotClicked(weapon.Name, weaponManager.weaponInventoryDictionary, UIManager));

                    // Also handle removing the weapon from the usable weapon lists
                    GameObject weaponGameObject = weaponManager.usableWeaponsAsGameObjectList.FirstOrDefault(wgo => wgo.name == weapon.Name);
                    if (weaponGameObject != null)
                    {
                        weaponManager.usableWeaponsAsGameObjectList.Remove(weaponGameObject);
                    }

                    for (int j = 0; j < weaponManager.usableWeaponInventoryEntries.Count; j++)
                    {
                        var entry = weaponManager.usableWeaponInventoryEntries[j];
                        if (entry.value == weapon)
                        {
                            weaponManager.usableWeaponInventoryEntries.Remove(entry);
                            weaponManager.usableWeaponDictionary.Remove(entry.key);

                            // Remove from the usable inventory
                            weaponGameObject.transform.SetParent(player.inventory.transform);

                            // Deactivate the game object
                            weaponGameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        buttonPanel.SetActive(false);
        inventoryButton.SetActive(false);
        useButton.SetActive(false);

        Debug.Log($"Added weapon: {weapon.Name} to inventory");
    }
}
