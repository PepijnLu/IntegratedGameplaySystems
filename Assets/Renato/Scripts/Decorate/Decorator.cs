using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class DecoratorAdd
{
    public abstract void AddToList<T>(List<T> list, T item);
    public abstract void RemoveFromList<T>(List<T> list, T item);
    public abstract void AddToDictionary<TValue>(Dictionary<string, TValue> dictionary, string key, TValue value);
    public abstract void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key);
}

public class ConcreteDecoratorAdd : DecoratorAdd
{
    public override void AddToList<T>(List<T> list, T item)
    {
        if(item is GameObject gameObject)
        {
            bool alreadyExist = list.OfType<GameObject>().Any(obj => obj.name == gameObject.name);

            if(!alreadyExist) 
            {
                list.Add(item);
                // Debug.Log($"Added {gameObject.name} to list. Message from Decorator class");
                gameObject.SetActive(false);
            }
            else
            {
                // Debug.Log($"{gameObject.name} already exists in the list. Message from Decorator class");
            }
        }
        else if (!list.Contains(item)) 
        {
            list.Add(item);
        }
    }

    public override void RemoveFromList<T>(List<T> list, T item)
    {
        // Remove from list logic
        if (list.Contains(item))
        {
            list.Remove(item);
        }
    }

    public override void AddToDictionary<TValue>(Dictionary<string, TValue> dictionary, string key, TValue value)
    {
        if(!dictionary.ContainsKey(key)) 
        {
            dictionary.Add(key, value);
        }
    }

    public override void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
    {
        if(dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }
}