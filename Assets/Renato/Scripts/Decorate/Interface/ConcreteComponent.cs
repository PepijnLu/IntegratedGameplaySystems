using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecoratorAdd decorator;
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
        decorator = new();
    }

    public void AddToList() 
    {
        decorator.AddToList(list, t);
    }

    public void DropFromList() 
    {
        decorator.RemoveFromList(list, t);
    }

    public TValue AddToDictionary<TValue>
    (
        Dictionary<string, TValue> dictionary, 
        string key, 
        List<TValue> allObjList,
        List<SerializableDictionary<TValue>> serializableDictionary
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
                    Debug.Log($"Updated existing entry for key: {key}");
                }
                else
                {
                    var newEntry = new SerializableDictionary<TValue>
                    {
                        key = key,
                        value = obj
                    };

                    serializableDictionary.Add(newEntry);
                    Debug.Log($"Added new entry for key: {key}");
                }

                return obj;
            }
        }

        Debug.LogWarning($"No matching object found in allObjList for key: {key}");
        return default;
    }


    public void RemoveDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value) 
    {
        decorator.RemoveFromDictionary(dictionary, key, value);
    }
}



// public class ConcreteComponentCombat : IComponentCombat 
// {
//     private ConcreteDecorator concreteDecorator;

// }
