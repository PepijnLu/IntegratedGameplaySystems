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
        if (r_WeaponsManager.usableWeapons.Count > 0)
        {
            currentWeapon = r_WeaponsManager.usableWeapons[0];
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
        foreach (var item in r_WeaponsManager.usableWeaponDictionary)
        {
            // string key = item.Key;
            R_Weapon w = item.Value;
            
            w.isActive = false;
            Debug.Log($"{w.Name} is deactivcated");
        }

        r_WeaponsManager.activeWeapon.Clear();
        
        // Find the currently active weapon, if any
        for (int i = 0; i < r_WeaponsManager.usableWeapons.Count; i++)
        {
            R_Weapon w = r_WeaponsManager.usableWeapons[i];
            if(w.isActive) 
            {
                currentWeaponIndex = i;
                currentWeapon = w;
                Debug.Log($"{w}");
                break;
            }

            // If no active weapon was found, select the first weapon in the list as the default
            if(currentWeapon == null) 
            {
                Debug.LogWarning("Current weapon is null");
                currentWeaponIndex = 0;
                currentWeapon = r_WeaponsManager.usableWeapons[0];
            }

            // Calculate the next weapon to switch to
            int nextWeaponIndex = (currentWeaponIndex + 1) % r_WeaponsManager.usableWeapons.Count;
            R_Weapon nextWeapon = r_WeaponsManager.usableWeapons[nextWeaponIndex];
            currentWeaponIndex = nextWeaponIndex;
            
            Debug.Log($"{nextWeapon} switched with {currentWeapon} with the number {currentWeaponIndex} as index");

            // Add the new active weapon to the list and mark it as active
            r_WeaponsManager.activeWeapon.Add(nextWeapon);
            nextWeapon.isActive = true;
        }
    }
}
