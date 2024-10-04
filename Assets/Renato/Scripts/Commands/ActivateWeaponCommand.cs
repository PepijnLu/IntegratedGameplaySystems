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
        
    }

    public void Undo()
    {
        
    }

    private void ActivateWeapn() 
    {
        // Usable Weapons
        // Usable Weapons UI
    }
}
