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

    public void CheckToAdd() 
    {
        Add(inventory, t);
    }

    public void CheckToRemove()
    {
        Drop(inventory, t);
    }
}

// public abstract class CombatDecorator 
// {
//     // Logic for the combat

// }