public class NormalCombatCommand : NormalAttack, ICommand
{
    public void Execute()
    {
        Attack();
    }

    public void Undo()
    {
        
    }
}