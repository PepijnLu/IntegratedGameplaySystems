using UnityEngine;

public class OpenInventoryCommandUI : ICommand
{
    private readonly GameObject weaponInventoryUI;

    public OpenInventoryCommandUI
    (
        GameObject weaponInventoryUI
    ) 
    {
        this.weaponInventoryUI = weaponInventoryUI;
    }

    public void Execute()
    {
        weaponInventoryUI.SetActive(true);
    }

    public void Undo()
    {
        weaponInventoryUI.SetActive(false);
    }
}