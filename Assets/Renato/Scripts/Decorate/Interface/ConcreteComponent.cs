using System.Collections.Generic;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecorator<T> concreteDecorator;
    private readonly List<T> inventory;
    private readonly T t;

    public ConcreteComponentAdd
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
        concreteDecorator = new(inventory, t);
        UnityEngine.Debug.Log($"concreteDecorator initialized: {concreteDecorator != null}");
    }

    public void Add() 
    {
        concreteDecorator.CheckToAdd();
    }

    public void Drop() 
    {
        concreteDecorator.CheckToRemove();
    }
}

// public class ConcreteComponentCombat : IComponentCombat 
// {
//     private ConcreteDecorator concreteDecorator;

// }
