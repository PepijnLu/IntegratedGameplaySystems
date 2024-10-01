using System.Collections.Generic;
public abstract class Decorator
{

    public abstract void AddToList<T>(List<T> list, T item);
    public abstract void RemoveFromList<T>(List<T> list, T item);
    public abstract void AddToDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value);
    public abstract void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value);
}

public class ConcreteDecorator : Decorator
{
    public override void AddToList<T>(List<T> list, T item)
    {
        // Add to list logic
        if (!list.Contains(item)) 
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

    public override void AddToDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if(!dictionary.ContainsKey(key)) 
        {
            dictionary.Add(key, value);
        }
    }


    public override void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if(dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
        }
    }

}
    // public override void AddToDictionary<T_Key, T_Value>(Dictionary<T_Key, T_Value> dictionary, T_Key key, T_Value value)
    // {
    //     // Add to dictionary logic
    //     if (!dictionary.ContainsKey(key))
    //     {
    //         dictionary.Add(key, value);
    //     }
    // }

    // public override void RemoveFromDictionary<T_Key, T_Value>(Dictionary<T_Key, T_Value> dictionary, T_Key key)
    // {
    //     // Remove from dictionary logic
    //     if (dictionary.ContainsKey(key))
    //     {
    //         dictionary.Remove(key);
    //     }
    // }


// public abstract class CombatDecorator 
// {
//     // Logic for the combat

// }
    //   w_gameManager.foundMatch = false;

    //             // Iterate through all weapons in w_gameManager
    //             foreach (R_Weapon weapon in w_gameManager.allWeapons)
    //             {
    //                 if(tempWeapon.name == weapon.Name) 
    //                 {
    //                     Debug.Log("Same weapon name as the scriptable object");
    //                     w_gameManager.foundMatch = true; 
    //                     break; 
    //                 }
    //             }

    //             if (!w_gameManager.foundMatch)
    //             {
    //                 Debug.Log("Not the same weapon name as the scriptable object");
    //             }

    //             return true;