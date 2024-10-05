using System.Collections.Generic;

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

public interface IComponentCombat 
{
    
}
