using System.Collections.Generic;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecorator concreteDecorator;
    private readonly List<T> list;
    private readonly T t;

    public ConcreteComponentAdd
    (
        List<T> list, 
        T t
    ) 
    {
        this.list = list;
        this.t = t;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        concreteDecorator = new();
        UnityEngine.Debug.Log($"concreteDecorator initialized: {concreteDecorator != null}");
    }

    public void Add() 
    {
        concreteDecorator.AddToList(list, t);
    }

    public void Drop() 
    {
        concreteDecorator.RemoveFromList(list, t);
    }

    public void AddDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value) 
    {
        concreteDecorator.AddToDictionary(dictionary, key, value);
    }

    public void RemoveDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value) 
    {
        concreteDecorator.RemoveFromDictionary(dictionary, key, value);
    }
}

    // w_gameManager.foundMatch = false;

    // // Iterate through all weapons in w_gameManager
    // foreach (R_Weapon weapon in w_gameManager.allWeapons)
    // {
    //     if(tempWeapon.name == weapon.Name) 
    //     {
    //         Debug.Log("Same weapon name as the scriptable object");
    //         w_gameManager.foundMatch = true; 
    //         break; 
    //     }
    // }

    // if (!w_gameManager.foundMatch)
    // {
    //     Debug.Log("Not the same weapon name as the scriptable object");
    // }





// public class ConcreteComponentCombat : IComponentCombat 
// {
//     private ConcreteDecorator concreteDecorator;

// }
