using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConcreteComponentAdd<T> : IComponentAdd
{
    private ConcreteDecorator decorator;
    private readonly List<T> list;
    private readonly T t;
    private WeaponGameManager weaponGameManager;
    private R_UIManager r_UIManager;

    public ConcreteComponentAdd
    (
        List<T> list, 
        T t,
        WeaponGameManager weaponGameManager
    ) 
    {
        this.list = list;
        this.t = t;
        this.weaponGameManager = weaponGameManager;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        decorator = new();
        r_UIManager = new();
    }

    public void Add() 
    {
        decorator.AddToList(list, t);
    }

    public void Drop() 
    {
        decorator.RemoveFromList(list, t);
    }

    public void AddDictionary<TValue>(
        Dictionary<string, TValue> dictionary, 
        string key, 
        List<TValue> allObjList
    ) where TValue : IIdentifiable
    {
        // Iterate over all objects in allObjList to find a match
        foreach (TValue obj in allObjList)
        {
            Debug.Log($"Checking object: {obj.Name} against key: {key}"); // Log comparison

            // Compare the name of the object with the key
            if (obj.Name.Equals(key.ToString(), StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison
            {
                decorator.AddToDictionary(dictionary, key, obj);

                // Check if the entry already exists
                var existingEntry = weaponGameManager.dictionaryEntries
                    .FirstOrDefault(entry => entry.key.Equals(key, StringComparison.OrdinalIgnoreCase));

                if (existingEntry != null)
                {
                    if (obj is R_Weapon weapon) // Cast obj to R_Weapon
                    {
                        existingEntry.value = weapon; // Assign if casting was successful
                        weapon.isAdded = true;
                        r_UIManager.InstantiateUISlot(weapon);

                        Debug.Log($"Updated existing entry for key: {key}");
                    }
                    else
                    {
                        Debug.LogWarning($"Cannot update existing entry for key: {key}. Obj is not of type R_Weapon.");
                    }
                }
                else
                {
                    // Create a new SerializableDictionary entry
                    if (obj is R_Weapon newWeapon) // Cast obj to R_Weapon
                    {
                        var entry = new SerializableDictionary<R_Weapon>
                        {
                            key = key,
                            value = newWeapon 
                        };
                        weaponGameManager.dictionaryEntries.Add(entry); // Add to the list of dictionary entries
                        Debug.Log($"Added new entry for key: {key}");
                        
                        newWeapon.isAdded = true;
                        r_UIManager.InstantiateUISlot(newWeapon);

                        // Log the current state of dictionaryEntries
                        Debug.Log("Current dictionary entries:");
                        foreach (var dictEntry in weaponGameManager.dictionaryEntries)
                        {
                            Debug.Log($"Key: {dictEntry.key}, Value: {dictEntry.value.Name}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Cannot add new entry for key: {key}. Obj is not of type R_Weapon.");
                    }
                }

                // Log the updated state of the dictionary
                Debug.Log("Updated dictionary state:");
                foreach (var kvp in dictionary)
                {
                    Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value.Name}");
                }

                return; // Exit after adding the object
            }
        }

        Debug.LogWarning($"No matching object found in allObjList for key: {key}");
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
