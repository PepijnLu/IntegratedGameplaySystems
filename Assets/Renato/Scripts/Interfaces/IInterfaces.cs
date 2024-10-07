using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombat 
{
    abstract IEnumerator ExecuteCombat(IAttackable attackable, R_WeaponsManager weaponManager, GameManager gameManager);
}

public interface ICommand
{
    abstract void Execute();
    abstract void Undo();
}

public class KeyCommand 
{
    public KeyCode key;
    public ICommand command;
}

public interface IAxisCommand 
{
    abstract float GetAxisValue();
}


/// <summary>
/// Interface for ScriptableObjects
/// </summary>
public interface IIdentifiable
{
    GameObject Obj { get; }
    string Name { get; }
    Sprite Icon { get; }
    bool IsAdded { set; }
    bool IsGrabable { get; set; }
}

public interface IComponentAdd
{
    void Add();
    void DropFromInventory();
    
    /// <summary>
    /// This method returns the type added in the dictionary. Also creates a new entry point inside the inspector for the dictionary.
    /// Need to pass in a dictionary, a string to compare with the key, a list to iterate through and a list of 'SerializableDictionary'
    /// </summary>
    TValue AddToDictionary<TValue>
    (
        Dictionary<string, TValue> dictionary, 
        string key, 
        List<TValue> allObjList,
        List<SerializableDictionary<string, TValue>> serializableDictionary
    ); 
}

public interface IAttackable
{
    Vector2 CombatStartPosition { get; }
    Vector2 CombatEndPosition { get; }
    Vector3 CombatStartRotation { get; }
    Vector3 CombatEndRotation { get; }
    float AttackRate { get; }
}

public interface IAttackableAbility 
{
    GameObject NormalAbilityPrefab { get; }
    float AbilityFireSpeed { get; }
    float AbilityLifeTime { get; } 
}


