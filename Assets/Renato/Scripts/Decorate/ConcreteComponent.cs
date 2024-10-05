using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecoratorAdd decorator;
    private readonly List<T> list;
    private readonly GameObject inventory;
    private readonly T t;

    public ConcreteComponentAdd
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
        decorator = new();
    }

    public void Add() 
    {
        decorator.AddToInventory(list, inventory, t);
    }

    public void DropFromInventory() 
    {
        decorator.RemoveFromInventory(list, t);
    }

    public TValue AddToDictionary<TValue>
    (
        Dictionary<string, TValue> dictionary, 
        string key, 
        List<TValue> allObjList,
        List<SerializableDictionary<string, TValue>> serializableDictionary
    )
    {
        foreach (TValue obj in allObjList)
        {
            string objName = null;

            if (obj is IIdentifiable identifiableObj)
            {
                objName = identifiableObj.Name; // Use the Name property from IIdentifiable
            }
            else if (obj is GameObject gameObject)
            {
                objName = gameObject.name; // Use the name property from GameObject
            }
            else
            {
                Debug.LogWarning($"Unsupported type: {typeof(TValue)}");
                continue;
            }

            if (objName.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                // Add to the dictionary using the key
                decorator.AddToDictionary(dictionary, key, obj);

                var existingEntry = serializableDictionary
                    .FirstOrDefault(entry => entry.key.Equals(key, StringComparison.OrdinalIgnoreCase));

                if (existingEntry != null)
                {
                    existingEntry.value = obj;
                    // Debug.Log($"Updated existing entry for key: {key}");
                }
                else
                {
                    var newEntry = new SerializableDictionary<string, TValue>
                    {
                        key = key,
                        value = obj
                    };

                    serializableDictionary.Add(newEntry);
                    // Debug.Log($"Added new entry for key: {key}");
                }

                return obj;
            }
        }

        Debug.LogWarning($"No matching object found in allObjList for key: {key}");
        return default;
    }
}



// public class ConcreteComponentCombat : IComponentCombat 
// {
//     private ConcreteDecorator concreteDecorator;

// }
