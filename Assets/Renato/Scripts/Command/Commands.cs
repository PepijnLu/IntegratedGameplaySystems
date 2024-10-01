using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AxisCommand : IAxisCommand
{
    private readonly string Value = "";

    public AxisCommand(string value) 
    {
        Value = value;
    }
    
    public float GetAxisValue() => Input.GetAxisRaw(Value);
}

public class AddCommand<T> : ICommand
{
    private ConcreteComponentAdd<T> component;
    private readonly List<T> list;
    private readonly List<IIdentifiable> identifiables;
    private readonly T t;
    private readonly Dictionary<string, IIdentifiable> dictionary;

    // private readonly Func<string, IIdentifiable> identifierLookup;

    public AddCommand
    (
        List<T> list,
        // List<IIdentifiable> identifiables, 
        T t,  
        Dictionary<string, IIdentifiable> dictionary
        // Func<string, IIdentifiable> identifierLookup
    ) 
    {
        this.list = list;
        // this.identifiables = identifiables;
        this.t = t;
        this.dictionary = dictionary;
        // this.identifierLookup = identifierLookup;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(list, t);
        Debug.Log($"component initialized: {component != null}");
    }

    public void Execute()
    {
        component.Add();
        AddToDictionary();
    }

    public void Undo()
    {
        component.Drop();
    }

    private void AddToDictionary()
    {
        if (t is GameObject gameObject)
        {
            string key = gameObject.name;

            // Cast the list items to IIdentifiable
            var allIdentifiables = identifiables.OfType<IIdentifiable>();

            foreach (var identifiable in allIdentifiables)
            {
                if (identifiable.Name == key)
                {
                    // If the dictionary doesn't already contain the key, add it
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, identifiable);
                        Debug.Log($"Added {key} to the dictionary with associated identifiable: {identifiable.Name}");
                    }
                    else
                    {
                        Debug.LogWarning($"The key {key} already exists in the dictionary.");
                    }

                    return; 
                }
            }

            Debug.LogWarning($"No identifiable found for the GameObject: {key}");
        }
    }


    // private void AddToDictionary(List<T> list, Dictionary<T, T> dictionary)
    // {
    //     if (t is GameObject gameObject)
    //     {
    //         string key = gameObject.name;

    //         var allIdentifiables = list.Cast<IIdentifiable>();

    //         foreach (var identifiable in allIdentifiables)
    //         {
    //             if(identifiable.Name == key) 
    //             {
    //                 if(!dictionary.ContainsKey(key)) 
    //                 {
    //                     dictionary.Add(key, identifiable);
    //                 }
    //                 else
    //                 {
    //                     Debug.LogWarning($"The key {key} already exists in the dictionary.");
    //                 }

    //                 return; // Exit once we've found a match
    //             }
    //         }

    //     }
    // }

    // // Lookup using the provided function
    // IIdentifiable identifiable = identifierLookup(key);
    // if (identifiable != null)
    // {
    //     // Here you would store the association, can be a simple Dictionary<string, IIdentifiable>
    //     // For example, you could define the dictionary outside of this class to keep it flexible
    //     Debug.Log($"Added {key} to the dictionary with associated identifiable.");
    //     // You can add to your shared dictionary here if you have a way to access it.
    // }
    // else
    // {
    //     Debug.LogWarning($"No identifiable found for the GameObject: {key}");
    // }

    
    // private void AddToDictionary(T item, Dictionary<string, IIdentifiable> dictionary) 
    // {
    //     // Here, you can use the item's name to add to the dictionary
    //     if (!dictionary.ContainsKey(item.Name)) 
    //     {
    //         dictionary.Add(item.Name, item);
    //         Debug.Log($"Added {item.Name} to the dictionary.");
    //     }
    //     else 
    //     {
    //         Debug.LogWarning($"Item with name {item.Name} already exists in the dictionary.");
    //     }
    // }

      
    // public void AddToDictionary(Dictionary<string, IIdentifiable> dictionary) 
    // {
    //     AddToDictionary(t, dictionary);
    // }

    public void AddToDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value, List<T> itemList)  
    {
        // foreach (IIdentifiable item in itemList.Select(v => (IIdentifiable)v))
        // {
        //     if(t.Name == item.Name) 
        //     {
        //         Debug.Log($"Names are the same: {t.Name}");
        //         component.AddDictionary(dictionary, key, value);
                
        //         break;
        //     }
        // }

    }

    public void RemoveFromDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value) 
    {
        component.RemoveDictionary(dictionary, key, value);
    }

}

public class SwitchWeaponCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class ActivateWeaponCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class CombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class NormalCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class BlockCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class AbilityCombatCommand : ICommand
{
    public void Execute()
    {
        
    }

    public void Undo()
    {
        
    }
}

public class AddWeaponToHUDCommand : R_UIManager, ICommand
{
    public void Execute()
    {
        // Add to inventory UI
    }

    public void Undo()
    {
        // Remove from inventory UI
    }
}
