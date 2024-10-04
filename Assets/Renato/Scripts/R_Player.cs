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
    private ICommand addCommand, removeCommand, addToInvCommandUI, openInvCommandUI, selectCommand;
    public IComponentAdd componentAdd;
    
    // Private readonly
    private readonly R_WeaponsManager w_gameManager;
    private readonly R_UIManager r_UIManager;
    private readonly Transform transform; // Player's transform
    private readonly float movementSpeed;

    private bool isInventoryOpen;
 
    // Constructor
    public R_Player
    (
        Transform transform, 
        float movementSpeed, 
        R_WeaponsManager w_gameManager,
        R_UIManager r_UIManager
    ) 
    {
        this.transform = transform;
        this.movementSpeed = movementSpeed;
        this.w_gameManager = w_gameManager; // Set the instance
        this.r_UIManager = r_UIManager;

        CustomAwake();
    }

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
        openInvCommandUI = new OpenInventoryCommandUI(r_UIManager.weaponInventoryUI);

        selectCommand = new SelectWeaponCommand
        (
            r_UIManager.slotDictionaryEntry,                // Class entry to store the values from the dictionary            
            r_UIManager.slotDictionaryUI,                   // Dictionary to store the UI slots
            w_gameManager.weaponInvDictionary,              // Dictionary that stores the weapons from the inventory
            r_UIManager.selectedSlot,                       // Slot of selected weapons
            r_UIManager.slotListUI,                         // List to store UI slots in
            r_UIManager.usableWeaponsUI,                    // List of usable weapons
            w_gameManager,                                  // WeaponGameManager class
            r_UIManager.slotPrefab                          // Placeholder for the UI slot prefab
        );
    } 

    public void CustomStart() 
    {
        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        input.BindInputToCommand(KeyCode.Tab, openInvCommandUI);
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
                    w_gameManager.weaponInvDictionary,          // Weapon's inventory dictionary
                    tempWeapon.name,                            // Weapon game object name
                    w_gameManager.allWeapons,                   // List of Weapon class
                    w_gameManager.inventoryEntries              // Serializable class that hold the keys and values of the dictionary
                );

                // Add to UI
                if(addedWeapon is IIdentifiable identifiable) 
                {
                    addToInvCommandUI = new AddToInventoryCommandUI<IIdentifiable>
                    (
                        r_UIManager.slotDictionaryEntry,        // Serializable class that hold the keys and values of the dictionary
                        r_UIManager.slotDictionaryUI,           // Dictionary for the UI slots
                        r_UIManager.slotListUI,                 // List for the UI slots
                        r_UIManager.weaponInventoryUI,          // Inventory for UI slots as a game object
                        r_UIManager.slotPrefabPath,             // Path to the prefab (resources)
                        identifiable,                           // Is addedWeapon
                        r_UIManager.slotPrefab                  // Placeholder for the UI slot prefab
                    );
                }

                // Add to UI inventory
                addToInvCommandUI.Execute();

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

        OpenInventoryUI(input);

        if(isInventoryOpen) 
        {
            selectCommand.Execute();
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

    public void OpenInventoryUI(R_InputHandler input) 
    {
        // Check if the Tab key is pressed
        if (input.keyCommands.Find(k => k.key == KeyCode.Tab)?.command == addCommand) 
        {
            // Check if the inventory is currently open
            if (openInvCommandUI != null && r_UIManager.weaponInventoryUI.activeInHierarchy) 
            {
                // If the inventory is open, close it
                Debug.Log("Closing inventory with Tab");
                openInvCommandUI.Undo(); // Close inventory
                isInventoryOpen = false;
            }
            else 
            {
                // If the inventory is not open, open it
                Debug.Log("Opening inventory with Tab");
                openInvCommandUI.Execute(); // Open inventory
                isInventoryOpen = true;
            }
        }
    }
}
