using UnityEngine;

[CreateAssetMenu(fileName = "NormalAttack", menuName = "Combat/Attacks/NormalAttack")]
public class NormalAttack : ScriptableObject, ICombat
{
    public void Attack() 
    {
        // Execute combat

        // If weapon type == sword then execute sword combat

        // else if weapon type == shield execute shield combat

        // esle weapon type == magic wand execute magic wand combat
    }

    public void ExecuteSwordCombat()
    {
        // Manage combat

        // Get the active weapon

        // Get the active game object

        // Move the game object to animate an attack move
    }

    public void ExecuteShieldCombat()
    {
        // Manage combat

        // Get the active weapon

        // Get the active game object

        // Move the game object to animate an attack move
    }

    public void ExecuteMagicWandCombat()
    {
        // Manage combat

        // Get the active weapon

        // Get the active game object

        // Move the game object to animate an attack move
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
    abstract void ExecuteSwordCombat();
    abstract void ExecuteShieldCombat();
    abstract void ExecuteMagicWandCombat();
}
