using System.Collections.Generic;
using UnityEngine;

public class AddToInventory<T> : ICommand
{
    private ComponentAdd<T> component;
    private readonly List<T> list;
    private readonly GameObject inventory;
    private readonly T t;

    public AddToInventory
    (
        List<T> list,
        GameObject inventory,
        T t
    ) 
    {
        this.list = list;
        this.inventory = inventory;
        this.t = t;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(list, inventory, t);
    }

    public void Execute()
    {
        component.Add();
    }

    public void Undo()
    {
        component.DropFromInventory();
    }
}
