using UnityEngine;

public class ActivateWeaponCommand : ICommand
{
    private readonly R_WeaponsManager r_WeaponsManager;
    private readonly R_UIManager r_UIManager;

    public ActivateWeaponCommand
    (
        R_WeaponsManager r_WeaponsManager,
        R_UIManager r_UIManager
    ) 
    {
        this.r_WeaponsManager = r_WeaponsManager;
        this.r_UIManager = r_UIManager;
    }

    public void Execute()
    {
        // ActivateWeapn();
    }

    public void Undo()
    {
        // DeactivateWeapon();
    }

    // private void DeactivateWeapon() 
    // {
    //     r_WeaponsManager.activeWeapon.isSelectedInUsable = false;
    //     r_WeaponsManager.activeWeapon.isActive = false;

    //     r_WeaponsManager.activeWeapon = null;
    //     r_WeaponsManager.activeGameObjectWeapon = null;
    // }

    // private void ActivateWeapn() 
    // {
    //     // Usable Weapons
    //     for (int i = 0; i < r_WeaponsManager.usableWeapons.Count; i++)
    //     {
    //         r_WeaponsManager.activeWeapon = r_WeaponsManager.usableWeapons[i];

    //         if(r_WeaponsManager.activeWeapon.isSelectedInUsable) 
    //         {
    //            r_WeaponsManager.activeWeapon.isActive = true; 
            
    //             for (int j = 0; j < r_UIManager.usableWeaponsUI.Count; j++)
    //             {
    //                 GameObject w_Obj = r_UIManager.usableWeaponsUI[j];
    //                 r_WeaponsManager.activeGameObjectWeapon = w_Obj;
    //             }
    //         }
    //     }   
    // }
}
