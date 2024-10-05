using UnityEngine;


[CreateAssetMenu(fileName = "NormalAttack", menuName = "Combat/Attacks/NormalAttack")]
public class NormalAttack : ScriptableObject, ICombat, ICommand
{
    public void Attack(R_Weapon weapon)
    {
        
    }

    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

[CreateAssetMenu(fileName = "SuperAttack", menuName = "Combat/Attacks/SuperAttack")]
public class SuperAttack : ScriptableObject, ICombat
{
    public void Attack(R_Weapon weapon)
    {
        
    }
}

[CreateAssetMenu(fileName = "SwordAbility", menuName = "Combat/Abilities/SwordAbility")]
public class SwordAbility : ScriptableObject, ICombat
{
    public void Attack(R_Weapon weapon)
    {
        
    }
}

[CreateAssetMenu(fileName = "SpearAbility", menuName = "Combat/Abilities/SpearAbility")]
public class SpearAbility : ScriptableObject, ICombat
{
    public void Attack(R_Weapon weapon)
    {
        
    }
}

public interface ICombat 
{
    abstract void Attack(R_Weapon weapon);
}
