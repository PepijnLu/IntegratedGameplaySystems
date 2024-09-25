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
    
    private readonly R_InputHandler input;
    private readonly ICommand command;
    private readonly List<T> inventory;
    private readonly T t;

    public AddCommand
    (
        R_InputHandler input, 
        ICommand command,
        List<T> inventory,
        T t
    ) 
    {
        this.input = input;
        this.command = command;
        this.inventory = inventory;
        this.t = t;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(input, command, inventory, t);
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
// Get from the decorate design pattern the method to check what the type of weapon it is to add

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
