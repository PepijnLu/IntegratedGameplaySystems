using UnityEngine;

public class SwapWeaponCommand : ICommand
{
    public R_Weapon currentWeapon;
    private readonly R_WeaponsManager r_WeaponsManager;
    private int currentWeaponIndex;

    public SwapWeaponCommand
    (
        R_WeaponsManager r_WeaponsManager
    ) 
    {
        this.r_WeaponsManager = r_WeaponsManager;

        // Initialize currentWeapon if the list is not empty
        if (r_WeaponsManager.usableScriptableWeapons.Count > 0)
        {
            currentWeapon = r_WeaponsManager.usableScriptableWeapons[0];
            Debug.Log($"Initialized currentWeapon: {currentWeapon.Name}");
        }
        else
        {
            Debug.LogWarning("No weapons available to initialize currentWeapon.");
        }
    }

    public void Execute()
    {
        SwapWeapon();
    }

    public void Undo()
    {
        
    }
    
    public void SwapWeapon() 
    {
        // Deactivate all weapons
        for (int i = 0; i < r_WeaponsManager.usableScriptableWeapons.Count; i++)
        {
            R_Weapon weapon = r_WeaponsManager.usableScriptableWeapons[i];
            GameObject weaponObj = r_WeaponsManager.usableGameObjectWeapons[i];

            weapon.isActive = false;
            weaponObj.SetActive(false); // Deactivate the corresponding GameObject
            Debug.Log($"{weapon.Name} is deactivated");
        }

        // Find the currently active weapon, if any
        for (int i = 0; i < r_WeaponsManager.usableScriptableWeapons.Count; i++)
        {
            R_Weapon w = r_WeaponsManager.usableScriptableWeapons[i];
            GameObject obj = r_WeaponsManager.usableGameObjectWeapons[i];

            if (w.isActive) 
            {
                currentWeaponIndex = i;
                currentWeapon = w;
                obj.SetActive(true); // Activate the currently active weapon's GameObject
                Debug.Log($"{currentWeapon.Name} is currently active with index {currentWeaponIndex}");

                break;
            }
        }

        // If no active weapon was found, select the first weapon in the list as the default
        if (currentWeapon == null) 
        {
            Debug.LogWarning("Current weapon is null");
            currentWeaponIndex = 0;

            if(r_WeaponsManager.usableScriptableWeapons.Count > 0) 
            {
                currentWeapon = r_WeaponsManager.usableScriptableWeapons[currentWeaponIndex];
                
                GameObject obj = r_WeaponsManager.usableGameObjectWeapons[currentWeaponIndex];
                obj.SetActive(true); // Activate the first weapon's GameObject by default
            } 
        }

        // Calculate the next weapon to switch to
        int nextWeaponIndex = (currentWeaponIndex + 1) % r_WeaponsManager.usableScriptableWeapons.Count;
        R_Weapon nextWeapon = r_WeaponsManager.usableScriptableWeapons[nextWeaponIndex];
        GameObject nextWeaponObj = r_WeaponsManager.usableGameObjectWeapons[nextWeaponIndex];
        
        // Update the current weapon index to reflect the new active weapon
        currentWeaponIndex = nextWeaponIndex;

        // Log the correct weapons and their indices
        Debug.Log($"{currentWeapon.Name} switched with {nextWeapon.Name} with the number {nextWeaponIndex} as index");

        // Deactivate all weapons first
        for (int i = 0; i < r_WeaponsManager.usableScriptableWeapons.Count; i++)
        {
            r_WeaponsManager.usableGameObjectWeapons[i].SetActive(false);
        }

        // Activate the next weapon and add it to the active weapon list
        r_WeaponsManager.activeWeapon.Clear(); // Clear the previous active weapon list
        r_WeaponsManager.activeWeapon.Add(nextWeapon); // Add the new active weapon
        nextWeapon.isActive = true;
        nextWeaponObj.SetActive(true); // Activate the corresponding GameObject
    }
}
