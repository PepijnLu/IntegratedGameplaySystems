using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator
{
    protected void Add<T>(List<T> inventory, T t) { if(!inventory.Contains(t)) inventory.Add(t); }
    protected void Drop<T>(List<T> inventory, T t) { if(inventory.Contains(t)) inventory.Remove(t); }
}

public class ConcreteDecorator<T> : Decorator
{
    private readonly List<T> inventory;
    private readonly T t;

    public ConcreteDecorator(List<T> inventory, T t) 
    {
        this.inventory = inventory;
        this.t = t;
    }

    public void CheckToAdd(R_InputHandler inputHandler, ICommand command) 
    {
        if(inputHandler.keyCommands.Find(k => k.key == KeyCode.E)?.command == command) 
        {
            Debug.Log($"The 'E' key is pressed, adding {t}...");
            Add(inventory, t);
        }
    }

    public void CheckToRemove(R_InputHandler inputHandler, ICommand command)
    {
        if(inputHandler.keyCommands.Find(k => k.key == KeyCode.R)?.command == command)  
        {
            Debug.Log($"The 'R' key is pressed, dropping {t}...");
            Drop(inventory, t);
        }
    }
}

// public abstract class CombatDecorator 
// {
//     // Logic for the combat

// }