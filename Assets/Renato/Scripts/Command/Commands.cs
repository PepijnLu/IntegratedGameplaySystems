using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class AxisCommand : IAxisCommand
{
    private readonly string Value = "";

    public AxisCommand(string value) 
    {
        Value = value;
    }
    
    public float GetAxisValue() => Input.GetAxisRaw(Value);
}

public class AddToListCommand<T, TIdentifiable> : ICommand where TIdentifiable : IIdentifiable
{
    private ConcreteComponentAdd<T> component;
    private readonly List<T> list;
    private readonly T t;

    public AddToListCommand
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
        component = new(list, t);
    }

    public void Execute()
    {
        component.AddToList();
    }

    public void Undo()
    {
        component.DropFromList();
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

public class AddToHUDCommand<T> : ICommand where T : IIdentifiable
{
    readonly public List<SerializableDictionary<GameObject>> serializableDictionary;
    readonly private Dictionary<string, GameObject> UI_SlotsDict;
    readonly private List<GameObject> UI_Slots;
    readonly private GameObject UI_Inv;
    readonly private string slotPrefabPath = "";
    readonly private T t;
    private GameObject slotPrefab;


    public AddToHUDCommand
    (
        List<SerializableDictionary<GameObject>> serializableDictionary,
        Dictionary<string, GameObject> UI_SlotsDict,
        List<GameObject> UI_Slots,
        GameObject UI_Inv,
        string slotPrefabPath,
        T t,
        GameObject slotPrefab
    ) 
    {
        this.serializableDictionary = serializableDictionary;
        this.UI_SlotsDict = UI_SlotsDict;
        this.UI_Slots = UI_Slots;
        this.UI_Inv = UI_Inv;
        this.slotPrefabPath = slotPrefabPath;
        this.t = t;
        this.slotPrefab = slotPrefab;
    } 

    public void Execute()
    {
        // Add to inventory UI
        InstantiateUISlot();
    }

    public void Undo()
    {
        // Remove from inventory UI
    }

    
    public void InstantiateUISlot()
    {
        // Image slotIcon;
        slotPrefab = Resources.Load<GameObject>(slotPrefabPath);
        
        // Instantiate the loaded prefab
        GameObject slot = GameObject.Instantiate(slotPrefab);

        // Get image component
        Image slotImage = slot.GetComponent<Image>();

        slot.transform.SetParent(UI_Inv.transform);
        slot.name = t.Name;

        // Assign the sprite from the Iidentifiable to the sprite of the slot image
        slotImage.sprite = t.Icon;

        UI_Slots.Add(slot);

        if(!UI_Inv) 
        {
            Debug.LogWarning("Failed to fetch the UI inventory");
        }

        if(!slot) 
        {
            Debug.LogWarning("Failed to load the slot UI prefab");
        }

        if(!UI_SlotsDict.ContainsKey(slot.name)) 
        {
            // Add it to the dictionary of UI Slots
            UI_SlotsDict.Add(slot.name, slot);
            
            // Create a new entry to store the name and the gameobject
            var entry = new SerializableDictionary<GameObject> 
            {   
                key = slot.name,
                value = slot
            };

            if(!serializableDictionary.Contains(entry)) 
            {
                serializableDictionary.Add(entry);
            }
        }
        else 
        {
            GameObject.Destroy(slot);
        }
    }
}
