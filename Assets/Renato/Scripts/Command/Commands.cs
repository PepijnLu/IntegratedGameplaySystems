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

public class AddCommand<T, TIdentifiable> : ICommand where TIdentifiable : IIdentifiable
{
    private ConcreteComponentAdd<T> component;
    private readonly List<T> list;
    private readonly T t;

    private readonly List<TIdentifiable> allObjList;
    private Dictionary<string, TIdentifiable> dictionary;

    private WeaponGameManager weaponGameManager;


    public AddCommand
    (
        List<T> list,
        T t,
        List<TIdentifiable> allObjList,
        Dictionary<string, TIdentifiable> dictionary,
        WeaponGameManager weaponGameManager

    ) 
    {
        this.list = list;
        this.t = t;
        this.allObjList = allObjList;
        this.dictionary = dictionary;
        this.weaponGameManager = weaponGameManager;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        component = new(list, t, weaponGameManager);
        // Debug.Log($"component initialized: {component != null}");
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
        if(t is GameObject gameObject) 
        {
            string key = gameObject.name;
            component.AddDictionary(dictionary, key, allObjList);
        }
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
