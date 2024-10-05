using UnityEngine;


[CreateAssetMenu(fileName = "NormalAttack", menuName = "Combat/Attacks/NormalAttack")]
public class NormalAttack : ScriptableObject
{
    private readonly NormalCombatCommand command;

    public void Attack() 
    {
        // Only perform an attack if the weapon is active

        // Fetch the weapon 
    }
}

// [CreateAssetMenu(fileName = "SuperAttack", menuName = "Combat/Attacks/SuperAttack")]
// public class SuperAttack : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
//         // Invoke normal attack command
//     }
// }

// [CreateAssetMenu(fileName = "SwordAbility", menuName = "Combat/Abilities/SwordAbility")]
// public class SwordAbility : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
        
//     }
// }

// [CreateAssetMenu(fileName = "SpearAbility", menuName = "Combat/Abilities/SpearAbility")]
// public class SpearAbility : ScriptableObject, ICombat
// {
//     public void Attack(R_Weapon weapon)
//     {
        
//     }
// }

public interface ICombat 
{
    abstract void ExecuteCombat();
}
