using UnityEngine;

[System.Serializable]
public class R_Player
{
    // SerializeField
    [SerializeField] private GameObject tempWeapon;

    // Public
    public GameObject prefab;
    public Transform spawnPoint;
    
    
    public GameObject inventory, usableInventory, activeWeaponSlot;

    // Private
    private R_InputHandler input; // Key input

    [Header("Commands")]
    private ICommand addCommand, removeCommand;
    private ICommand addToInvCommandUI, openInvCommandUI;
    private ICommand selectCommand;
    private ICommand switchWeaponCommand;
    private IComponentAdd componentAdd;
    
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
        addCommand = new AddToInventory<GameObject>
        (
            w_gameManager.weaponInventoryList,
            inventory, 
            null
        );

        // Remove from list command
        removeCommand = new AddToInventory<GameObject>
        (
            w_gameManager.weaponInventoryList,
            inventory,
            null
        );
        
        componentAdd = new ConcreteComponentAdd<GameObject>
        (
            w_gameManager.weaponInventoryList,
            inventory, 
            null
        );

        openInvCommandUI = new OpenInventoryCommandUI(r_UIManager.weaponInventoryUI);

        selectCommand = new SelectWeaponCommand
        (
            this,
            r_UIManager.selectedSlot,       // Slot of selected weapons
            w_gameManager,                  // Weapon game manager class
            r_UIManager                     // UI manager class
        );


        addToInvCommandUI = new AddToInventoryCommandUI<IIdentifiable>
        (
            w_gameManager,
            r_UIManager,
            r_UIManager.slotPrefabPath,             // Path to the prefab (resources)
            null                                    // Is addedWeapon
        );

        switchWeaponCommand = new SwapWeaponCommand 
        (
            w_gameManager
        );

        // Get the inventory

    } 

    public void CustomStart() 
    {
        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        input.BindInputToCommand(KeyCode.Tab, openInvCommandUI);
        input.BindInputToCommand(KeyCode.Q, switchWeaponCommand);
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
                addCommand = new AddToInventory<GameObject>
                (
                    w_gameManager.weaponInventoryList, 
                    inventory,
                    tempWeapon
                );

                // Add weapon to list if weapon is not active
                for (int i = 0; i < w_gameManager.allWeapons.Count; i++)
                {
                    var w = w_gameManager.allWeapons[i];
                    if(w.Name == tempWeapon.name) 
                    {
                        if(w.isGrabable) 
                        {
                            addCommand.Execute();
                            w.isGrabable = false;

                            break;
                        }
                    }
                }
                

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
                        w_gameManager,
                        r_UIManager,
                        r_UIManager.slotPrefabPath,             // Path to the prefab (resources)
                        identifiable                            // Is addedWeapon
                    );
                }

                // Add to UI inventory
                addToInvCommandUI.Execute();
            }
        }

        if(input.keyCommands.Find(k => k.key == KeyCode.Q)?.command == addCommand) 
        {
            switchWeaponCommand.Execute();
        }
       
        OpenInventoryUI(input);

        if(isInventoryOpen) 
        {
            selectCommand.Execute();
            addToInvCommandUI.Undo();
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
                    return false;
                }

                return true;
            }
            else 
            {
                tempWeapon = null;
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
                openInvCommandUI.Undo(); // Close inventory
                isInventoryOpen = false;
            }
            else 
            {
                // If the inventory is not open, open it
                openInvCommandUI.Execute(); // Open inventory
                isInventoryOpen = true;
            }
        }
    }
}
