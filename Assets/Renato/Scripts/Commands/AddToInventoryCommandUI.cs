using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AddToInventoryCommandUI<T> : ICommand where T : IIdentifiable
{
    readonly public List<SerializableDictionary<GameObject>> serializableDictionary;
    readonly private Dictionary<string, GameObject> Slot_DictionaryUI;
    readonly private List<GameObject> UI_Slots;
    readonly private GameObject UI_Inv;
    readonly private string slotPrefabPath = "";
    readonly private T t;
    private GameObject slotPrefab;


    public AddToInventoryCommandUI
    (
        List<SerializableDictionary<GameObject>> serializableDictionary,
        Dictionary<string, GameObject> Slot_DictionaryUI,
        List<GameObject> UI_Slots,
        GameObject UI_Inv,
        string slotPrefabPath,
        T t,
        GameObject slotPrefab
    ) 
    {
        this.serializableDictionary = serializableDictionary;
        this.Slot_DictionaryUI = Slot_DictionaryUI;
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

        if(!Slot_DictionaryUI.ContainsKey(slot.name)) 
        {
            // Add it to the dictionary of UI Slots
            Slot_DictionaryUI.Add(slot.name, slot);
            
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
