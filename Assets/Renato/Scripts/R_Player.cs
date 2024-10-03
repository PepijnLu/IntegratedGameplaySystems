using System.Linq;
using UnityEngine;

[System.Serializable]
public class R_Player
{
    // SerializeField
    [SerializeField] private GameObject activeWeapon;
    [SerializeField] private GameObject tempWeapon;
    [SerializeField] private bool inRange;

    // Public
    public GameObject prefab;
    public Transform spawnPoint;
    

    // Private
    private R_InputHandler input; // Key input
    private ICommand addCommand, removeCommand, HUDCommand;
    public IComponentAdd componentAdd;
    
    // Private readonly
    private readonly WeaponGameManager w_gameManager;
    private readonly R_UIManager r_UIManager;
    private readonly Transform transform; // Player's transform
    private readonly float movementSpeed;
 
    // Constructor
    public R_Player
    (
        Transform transform, 
        float movementSpeed, 
        WeaponGameManager w_gameManager,
        R_UIManager r_UIManager
    ) 
    {
        this.transform = transform;
        this.movementSpeed = movementSpeed;
        this.w_gameManager = w_gameManager; // Set the instance
        this.r_UIManager = r_UIManager;

        CustomAwake();
    }

    public R_Player(){}

    public void CustomAwake() 
    {
        // Input key
        input = new();

        // Add to list command
        addCommand = new AddToListCommand<GameObject, R_Weapon>
        (
            w_gameManager.weaponInventory, 
            null
        );

        // Remove from list command
        removeCommand = new AddToListCommand<GameObject, R_Weapon>
        (
            w_gameManager.weaponInventory, 
            null
        );

        componentAdd = new ConcreteComponentAdd<GameObject>(w_gameManager.weaponInventory, null);
    } 

    public void CustomStart() 
    {
        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        
        // Debug.Log($"addCommand initialized: {addCommand != null}");
        // Debug.Log($"removeCommand initialized: {removeCommand != null}");
        Debug.Log($"componentAdd initialized: {componentAdd != null}");


    }
    
    public void CustomUpdate() 
    {
        addCommand = input.HandleKeyInput();

        input.HandleMovement
        (
            new AxisCommand("Horizontal"), 
            new AxisCommand("Vertical"),
            transform,
            movementSpeed
        );

        if(InRangeOfWeapon()) 
        {            
            if(input.keyCommands.Find(k => k.key == KeyCode.E)?.command == addCommand) 
            {
                addCommand = new AddToListCommand<GameObject, R_Weapon>
                (
                    w_gameManager.weaponInventory, 
                    tempWeapon
                );
                
                // Add to list
                addCommand.Execute();

                // var weaponInvDictAsRWeapon = w_gameManager.weaponInvDict
                // .Where(kvp => kvp.Value is R_Weapon)
                // .ToDictionary(kvp => kvp.Key, kvp => (R_Weapon)kvp.Value);

                // Add to dictionary
                var addedWeapon = componentAdd.AddToDictionary
                (
                    w_gameManager.weaponInvDict,                // Weapon's inventory dictionary
                    tempWeapon.name,                            // Weapon game object name
                    w_gameManager.allWeapons,                   // List of Weapon class
                    w_gameManager.inventoryEntries              // Serializable class that hold the keys and values of the dictionary
                );

                // Add to UI
                if(addedWeapon is IIdentifiable identifiable) 
                {
                    HUDCommand = new AddToHUDCommand<IIdentifiable>
                    (
                        r_UIManager.UISlotDictionaryEntries,    // Serializable class that hold the keys and values of the dictionary
                        r_UIManager.UI_SlotsDict,               // Dictionary for the UI slots
                        r_UIManager.UI_SlotsList,               // List for the UI slots
                        r_UIManager.UI_Inv,                     // Inventory for UI slots as a game object
                        r_UIManager.slotPrefabPath,             // Path to the prefab (resources)
                        identifiable,                           // Is addedWeapon
                        r_UIManager.slotPrefab
                    );
                }

                // Add to UI inventory
                HUDCommand.Execute();

            }
        }

        // Removing the weapon if the player has weapons in their inventory
        if(input.keyCommands.Find(k => k.key == KeyCode.R)?.command == addCommand) 
        {
            if (w_gameManager.weaponInventory.Count > 0) 
            {                
                GameObject weaponToRemove = w_gameManager.weaponInventory[^1];
                removeCommand = new AddToListCommand<GameObject, R_Weapon>
                (
                    w_gameManager.weaponInventory,            // Weapon inventory
                    weaponToRemove                            // Weapon to remove from the inventory
                );

                removeCommand.Undo();
            }
        }
    }

    
    public bool InRangeOfWeapon()
    {
        // Get the collider from the weapon
        if (!transform.TryGetComponent<CircleCollider2D>(out var p_collider)) return false;

        // Do something
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, p_collider.radius);

        foreach (var collider in hitCollider)
        {
            if(collider != null && collider.CompareTag("Weapon")) 
            {
                tempWeapon = collider.gameObject;
                inRange = true;

                bool foundMatch = false;

                // Iterate through all weapons in w_gameManager
                foreach (R_Weapon weapon in w_gameManager.allWeapons)
                {
                    if(tempWeapon.name == weapon.Name) 
                    {
                        foundMatch = true;

                        break; 
                    }
                }

                if (!foundMatch)
                {
                    // Debug.Log("Not the same weapon name as the scriptable object");
                    return false;
                }

                return true;
            }
            else 
            {
                tempWeapon = null;
                inRange = false;
            }
        }

        return false;
    }
}
