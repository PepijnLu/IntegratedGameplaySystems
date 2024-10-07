using System.Linq;
using UnityEngine;

public class SwapWeaponCommand : ICommand
{
    private R_Weapon currentWeapon;
    private readonly R_WeaponsManager weaponManager;
    string currentWeaponKey;

    public SwapWeaponCommand
    (
        R_WeaponsManager weaponManager
    ) 
    {
        this.weaponManager = weaponManager;

        if (weaponManager.usableWeaponDictionary.Count > 0)
        {
            var firstEntry = weaponManager.usableWeaponDictionary.First();
            currentWeapon = firstEntry.Value;
        }
    }

    public void Execute()
    {
        if(weaponManager.usableWeaponDictionary.Count > 0 
        && weaponManager.usableWeaponDictionary.Count <= 2) 
        {
            SwapWeapon();
        }

    }

    public void Undo()
    {
        
    }

    private void SwapWeapon() 
    {
        R_Weapon weapon = null;
        GameObject weaponGameObject = null;

        // Deactivate all weapons
        foreach (var element in weaponManager.usableWeaponDictionary)
        {
            weapon = element.Value;
            weapon.isActive = false;

            // Fetch the corresponding GameObject from the list by matching the name
            foreach (var weaponGameObjectElement in weaponManager.weaponsInventoryAsGameObjectList)
            {
                if(weaponGameObjectElement.name == weapon.Name) 
                {
                    weaponGameObject = weaponGameObjectElement;
                    weaponGameObject.SetActive(false);  // Deactivate GameObject
                    Debug.Log($"{weapon.Name} is deactivated");
                    break;
                }
            }    
        }

        // Find the currently active weapon, if any
        foreach (var element in weaponManager.usableWeaponDictionary)
        {
            if (element.Value.isActive) 
            {
                currentWeaponKey = element.Key; 
                currentWeapon = element.Value;

                weaponGameObject = weaponManager.weaponsInventoryAsGameObjectList.First(wgo => wgo.name == currentWeapon.Name);
                weaponGameObject.SetActive(true);  // Activate the current weapon's GameObject
                Debug.Log($"{currentWeapon.Name} is currently active with key {currentWeaponKey}");
                break;
            }
        }

        // If no active weapon was found, select the first weapon in the dictionary as the default
        if (currentWeapon == null) 
        {
            Debug.LogWarning("Current weapon is null");

            if(weaponManager.usableWeaponDictionary.Count > 0) 
            {
                currentWeaponKey = weaponManager.usableWeaponDictionary.Keys.First(); 
                currentWeapon = weaponManager.usableWeaponDictionary[currentWeaponKey];
                    
                // Activate the corresponding GameObject
                weaponGameObject = weaponManager.weaponsInventoryAsGameObjectList.First(wgo => wgo.name == currentWeapon.Name);
                weaponGameObject.SetActive(true);
            }
        }

        // Calculate the next weapon to switch to
        var keysList = weaponManager.usableWeaponDictionary.Keys.ToList();
        int currentKeyIndex = keysList.IndexOf(currentWeaponKey);
        
        // Ensure we advance to the next weapon
        int nextWeaponIndex = (currentKeyIndex + 1) % keysList.Count;
        string nextWeaponKey = keysList[nextWeaponIndex];
        R_Weapon nextWeapon = weaponManager.usableWeaponDictionary[nextWeaponKey];
        GameObject nextWeaponObj = weaponManager.weaponsInventoryAsGameObjectList.First(wgo => wgo.name == nextWeapon.Name);
        
        // Update the current weapon key to reflect the new active weapon
        currentWeaponKey = nextWeaponKey;  // Update to the next weapon's key
        currentWeapon = nextWeapon;  // Update to the next weapon
        currentWeapon.isActive = true;  // Set the next weapon as active

        // Log the correct weapons and their indices
        Debug.Log($"{currentWeapon.Name} switched with {nextWeapon.Name} with the index {nextWeaponIndex}");

        // Deactivate all weapons first
        foreach (var element in weaponManager.weaponsInventoryAsGameObjectList)
        {
            element.SetActive(false);  // Deactivate all GameObjects
        }

        weaponManager.activeWeaponGameObject = null;
        weaponManager.activeWeaponGameObject = nextWeaponObj;
        
        weaponManager.activeWeapon = null;
        weaponManager.activeWeapon = nextWeapon;
        nextWeaponObj.SetActive(true);  // Activate the corresponding GameObject
    }
}
