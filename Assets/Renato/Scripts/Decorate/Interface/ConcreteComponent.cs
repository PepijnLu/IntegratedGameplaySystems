using System.Collections.Generic;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecorator<T> concreteDecorator;
    private readonly List<T> inventory;
    private readonly T t;
    private readonly R_InputHandler inputHandler;
    private readonly ICommand command;

    public ConcreteComponentAdd
    (
        R_InputHandler inputHandler, 
        ICommand command, 
        List<T> inventory, 
        T t
    ) 
    {
        this.inputHandler = inputHandler;
        this.command = command;
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
        concreteDecorator.CheckToAdd(inputHandler, command);
    }

    public void Drop() 
    {
        concreteDecorator.CheckToRemove(inputHandler, command);
    }
}

// public class ConcreteComponentCombat : IComponentCombat 
// {
//     private ConcreteDecorator concreteDecorator;

// }
