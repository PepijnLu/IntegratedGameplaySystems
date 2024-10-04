using System.Collections.Generic;
using UnityEngine.UI;
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

public class OpenInventoryCommandUI : ICommand
{
    private readonly GameObject weaponInventoryUI;

    public OpenInventoryCommandUI
    (
        GameObject weaponInventoryUI
    ) 
    {
        this.weaponInventoryUI = weaponInventoryUI;
    }

    public void Execute()
    {
        weaponInventoryUI.SetActive(true);
    }

    public void Undo()
    {
        weaponInventoryUI.SetActive(false);
    }
}

public class SelectWeaponCommand: ICommand
{
    private readonly List<SerializableDictionary<GameObject>> slotDictionaryEntry;
    private readonly Dictionary<string, GameObject> Slot_DictionaryUI;
    private readonly Dictionary<string, R_Weapon> weaponInvDictionary;
    private readonly List<R_Weapon> selectedSlot;
    private readonly List<GameObject> slotListUI;
    private readonly List<GameObject> usableWeaponsUI;
    private readonly R_WeaponsManager weaponGameManager;
    private readonly GameObject slotPrefab;

    public SelectWeaponCommand
    (
        List<SerializableDictionary<GameObject>> slotDictionaryEntry,
        Dictionary<string, GameObject> slotDictionaryUI,
        Dictionary<string, R_Weapon> weaponInvDictionary,
        List<R_Weapon> selectedSlot,
        List<GameObject> slotListUI,
        List<GameObject> usableWeaponsUI,
        R_WeaponsManager weaponGameManager,
        GameObject slotPrefab
    ) 
    {
        this.slotDictionaryEntry = slotDictionaryEntry;
        this.Slot_DictionaryUI = slotDictionaryUI;
        this.weaponInvDictionary = weaponInvDictionary;
        this.selectedSlot = selectedSlot;
        this.slotListUI = slotListUI;
        this.usableWeaponsUI = usableWeaponsUI;
        this.weaponGameManager = weaponGameManager;
        this.slotPrefab = slotPrefab;
    }

    public void Execute()
    {
        Select(weaponInvDictionary, weaponGameManager);
    }

    public void Undo()
    {
        
    }

    public void Select(Dictionary<string, R_Weapon> weaponInvDict, R_WeaponsManager weaponGameManager)  
    {
        // Iterate through the UI dictionary
        foreach (var slot in Slot_DictionaryUI)
        {
            // Assign the key and value from dictionary
            string slotName = slot.Key;
            GameObject slotObject = slot.Value;

            // Get the button component
            Button slotButton = slotObject.GetComponent<Button>();
            
            // Remove any previous listeners to avoid duplicate actions
            slotButton.onClick.RemoveAllListeners();

            // Add a new listener to handle the click event
            slotButton.onClick.AddListener(() => OnSlotClicked(slotName, weaponInvDict, weaponGameManager));   
        }
    }

    private void OnSlotClicked(string slotName, Dictionary<string, R_Weapon> dictionary, R_WeaponsManager weaponGameManager) 
    {
        Debug.Log($"Clicked on slot: {slotName}");

        // Check if the slot exists in the weapon inventory dictionary
        if (dictionary.TryGetValue(slotName, out R_Weapon w)) 
        {
            // Get the previously selected weapon if there is one, otherwise set it to null
            R_Weapon previousW = selectedSlot.Count > 0 ? selectedSlot[0] : null;

            // If no weapon is selected or if it's a new weapon
            if (!selectedSlot.Contains(w)) 
            {
                // If a weapon was previously selected, deselect it
                if (previousW != null) 
                {
                    previousW.selected = false;
                    selectedSlot.Remove(previousW);  // Remove previous weapon from selection
                    Debug.Log($"Deselected weapon: {previousW.Name}");
                }

                // Add the new weapon to the selected slot and mark it as selected
                selectedSlot.Add(w);
                OpenSelectPanel(weaponGameManager);
                w.selected = true;
                Debug.Log($"Selected weapon: {w.Name}");
            }
            else
            {
                // If the weapon is already selected, deselect it
                w.selected = false;
                selectedSlot.Remove(w);
                Debug.Log($"Deselected weapon: {w.Name}");
            }
        }
        else 
        {
            Debug.LogWarning($"No weapon found for slot: {slotName}");
        }
    }

    private void OpenSelectPanel(R_WeaponsManager weaponGameManager) 
    {
        foreach (var w in selectedSlot)
        {
            if(w.selected) 
            {
                foreach (var slot in slotListUI)
                {
                    if (slot == null)
                    {
                        Debug.LogWarning($"Slot {slot.name} is null or not initialized properly");
                        return;
                    }

                    if(w.Name == slot.name) 
                    {
                        Transform selectPanelTransform = slot.transform.Find("Select Panel");

                        if(selectPanelTransform == null) 
                        {
                            Debug.LogWarning("No select panel couldnt be found!");
                            return;
                        }

                        GameObject selectPanel = selectPanelTransform.gameObject;
                        Debug.Log($"{selectPanel.name} has been found under slot: {slot.name}");

                        if (selectPanel.activeSelf)
                        {
                            Debug.Log($"{selectPanel.name} is already active.");
                        }
                        else
                        {
                            selectPanel.SetActive(true);
                            Debug.Log($"{selectPanel.name} is now set to active.");
                        }

                        if(!slot.TryGetComponent<Button>(out var btn)) 
                        {
                            Debug.Log($"No button to be found on the {slotPrefab.name}");
                            return;
                        }

                        Debug.Log($"{btn.name} is not null");
                        btn.enabled = false;

                        // Add event listener to the button of the selectPanel
                        if(!selectPanel.TryGetComponent<Button>(out var btn2)) 
                        {
                            Debug.LogWarning($"No button to be found on the {selectPanel.name}");
                            return;
                        } 

                        Debug.Log($"{btn2.name} on the slot has been found");

                        btn2.onClick.RemoveAllListeners();
                        btn2.onClick.AddListener(() => OnAddClick(weaponGameManager));
                    }
                    else 
                    {
                        Debug.LogWarning("Names are not the same");
                    }
                }
            }
            else 
            {
                return;
            }

        }
    }

    private void OnAddClick(R_WeaponsManager weaponGameManager) 
    {
        // Add the weapon from the weapon inventory list to the list of usable weapons
        R_Weapon w = selectedSlot[0];

        // Add to the list and dictionary
        if(!weaponGameManager.usableWeapons.Contains(w) && weaponGameManager.usableWeapons.Count < 2) 
        {
            weaponGameManager.usableWeapons.Add(w);
            weaponGameManager.usableWeapnDictionary.Add(w.Name, w); 
        }

        // Create a new entry point for the dictionary
        var entry = new SerializableDictionary<R_Weapon> 
        {   
            key = w.Name,
            value = w
        };

        // Add it in the list of usable entries
        weaponGameManager.usableEntries.Add(entry);

        // Iterate through the UI slot list
        for (int i = 0; i < slotListUI.Count; i++)
        {
            GameObject slot = slotListUI[i];
            if(slot.name == w.Name) 
            {
                if(usableWeaponsUI.Count < 2 && !usableWeaponsUI.Contains(slot)) 
                {
                    usableWeaponsUI.Add(slot);
                }
            }
        }

        // Remove it from the weapon inventory list of game objects
        for (int i = 0; i < weaponGameManager.weaponInventory.Count; i++)
        {
            GameObject _w = weaponGameManager.weaponInventory[i];
            if(_w.name == w.Name) 
            {
                weaponGameManager.weaponInventory.Remove(_w);
            }
        }

        // Remove it from the inventory dictionary
        if(weaponGameManager.weaponInvDictionary.ContainsKey(w.Name)) 
        {
            weaponGameManager.weaponInvDictionary.Remove(w.Name);
        }

        // Remove it from the weapon inventory entry
        for (int i = 0; i < weaponGameManager.inventoryEntries.Count; i++)
        {
            if(weaponGameManager.inventoryEntries[i].key == w.Name) 
            {
                weaponGameManager.inventoryEntries.Remove(weaponGameManager.inventoryEntries[i]);
            }
        }

        // Remove it from the UI slot dictionary
        for (int i = 0; i < slotListUI.Count; i++)
        {
            GameObject slot = slotListUI[i];
            if(slot.name == w.Name) 
            {
                slotListUI.Remove(slot);
            }
        }

        // Remove the UI slot from the dictionary
        if(Slot_DictionaryUI.ContainsKey(w.Name)) 
        {
            Slot_DictionaryUI.Remove(w.Name);
        }

        // Remove it from the UI slot dicrionary entry
        for (int i = 0; i < slotDictionaryEntry.Count; i++)
        {
            if(slotDictionaryEntry[i].key == w.Name) 
            {
                slotDictionaryEntry.Remove(slotDictionaryEntry[i]);
            }
        }

        w.selected = false;
        selectedSlot.Remove(w);

        // Destroy the slot or move it under a new game object of UI
    }
}

