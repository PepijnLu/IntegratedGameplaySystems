using System.Collections;
using UnityEngine;

public class AttackCommand : NormalAttack, ICommand
{
    private R_GameManager gameManager;
    private R_WeaponsManager weaponManager;

    public void Initialize(R_GameManager gameManager, R_WeaponsManager weaponManager)
    {
        this.gameManager = gameManager;
        this.weaponManager = weaponManager;
    }

    public void Execute()
    {
        IAttackable attackable = weaponManager.activeWeapon;
        IAttackableAbility attackableAbility = weaponManager.activeWeapon;
        // If the active weapon implements IAttackable, call Attack
        if (attackable != null)
        {
            Attack(attackable, attackableAbility, weaponManager, gameManager);
        }
        else
        {
            Debug.LogError("Active weapon does not implement IAttackable.");
        }
    }
    

    public void Undo()
    {
        
    }
}

public interface ICombat 
{
    abstract IEnumerator ExecuteCombat(IAttackable attackable, R_WeaponsManager weaponManager, R_GameManager gameManager);
}