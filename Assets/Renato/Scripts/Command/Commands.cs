using System.Collections.Generic;
using UnityEngine;

public class AxisCommand : IAxisCommand
{
    private readonly string Value = "";

    public AxisCommand(string value) 
    {
        Value = value;
    }
    
    public float GetAxisValue() => Input.GetAxisRaw(Value);
}

public class AddCommand<T> : ICommand
{
    private ConcreteComponentAdd<T> component;
    private readonly List<T> inventory;
    private readonly T t;

    public AddCommand
    (
        List<T> inventory,
        T t
    ) 
    {
        this.inventory = inventory;
        this.t = t;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(inventory, t);
        Debug.Log($"component initialized: {component != null}");
    }

    public void Execute()
    {
        component.Add();
    }

    public void Undo()
    {
        component.Drop();
    }
}

public class SwitchWeaponCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class ActivateWeaponCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class CombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class NormalCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class BlockCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class AbilityCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class AddWeaponToHUDCommand : R_UIManager, ICommand
{
    public void Execute()
    {
        // Add to inventory UI
    }

    public void Undo()
    {
        // Remove from inventory UI
    }
}
