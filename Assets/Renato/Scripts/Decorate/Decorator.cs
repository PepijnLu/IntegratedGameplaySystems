using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class DecoratorAdd
{
    public abstract void AddToInventory<T>(List<T> list, GameObject inventory, T item);
    public abstract void RemoveFromInventory<T>(List<T> list, T item);
    public abstract void AddToDictionary<TValue>(Dictionary<string, TValue> dictionary, string key, TValue value);
    public abstract void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key);
}

public class ConcreteDecoratorAdd : DecoratorAdd
{
    public override void AddToInventory<T>(List<T> list, GameObject inventory, T item)
    {
        if(item is GameObject gameObject)
        {
            bool alreadyExist = list.OfType<GameObject>().Any(obj => obj.name == gameObject.name);

            if(!alreadyExist) 
            {
                list.Add(item);
                gameObject.transform.SetParent(inventory.transform);

                // Debug.Log($"Added {gameObject.name} to list. Message from Decorator class");
                gameObject.SetActive(false);
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                gameObject.transform.localPosition = new(.15f, 1.8f);
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

    public override void RemoveFromInventory<T>(List<T> list, T item)
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