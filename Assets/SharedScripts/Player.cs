using System.Collections.Generic;
using UnityEngine;

public class Player : EventUser
{
    [SerializeField] private GameObject tempWeapon;
    private readonly R_WeaponsManager weaponManager;
    private readonly R_UIManager UIManager;
    private readonly Transform transform; // Player's transform
    private float movementSpeed;
    private bool isInventoryOpen;
    private GameManager gameManager;
    private PlayerStats playerStats;
    private R_InputHandler input; // Key input
    public Transform spawnPoint;    
    public GameObject inventory, usableInventory, activeWeaponSlot;
    public StateMachine stateMachine;
    public Dictionary<string, PlayerState> playerStates;

    [Header("Commands")]
    private ICommand addCommand, removeCommand;
    private ICommand addToInvCommandUI, openInvCommandUI;
    private ICommand selectCommand;
    private ICommand switchWeaponCommand, activateWeaponCommand;
    private AttackCommand normalAttackCommand;
    private IComponentAdd componentAdd;

    public Player
    (
        GameManager gameManager,
        R_WeaponsManager weaponManager,
        R_UIManager UIManager,
        Transform transform,
        float movementSpeed
    ) 
    {   
        this.gameManager = gameManager;
        this.weaponManager = weaponManager;
        this.UIManager = UIManager;
        this.transform = transform;
        this.movementSpeed = movementSpeed;

        CustomAwake();        
    }

    public void CustomAwake() 
    {
        // PEPIJN
        //Give the player stats and a statemachine
        playerStats = new();
        stateMachine = new(this);

        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);

        //Set the beginning survival states
        playerStates = new()
        {
            ["HungerState"] = stateMachine.playerStateTypes["NormalHungerState"],
            ["HealthState"] = stateMachine.playerStateTypes["NormalHealthState"],
            ["ThirstState"] = stateMachine.playerStateTypes["NormalThirstState"],
        };

        // Input key
        input = new();

        if(input == null) 
        {
            Debug.Log("Input Handler not found");
        }

        // Add to list command
        addCommand = new AddToInventory<GameObject>
        (
            weaponManager.weaponsInventoryAsGameObjectList,
            inventory, 
            null
        );

        // Remove from list command
        removeCommand = new AddToInventory<GameObject>
        (
            weaponManager.weaponsInventoryAsGameObjectList,
            inventory,
            null
        );
        
        componentAdd = new ComponentAdd<GameObject>
        (
            weaponManager.weaponsInventoryAsGameObjectList,
            inventory, 
            null
        );

        openInvCommandUI = new OpenInventoryCommandUI(UIManager.UI_weaponInventory);

        selectCommand = new AddToUsableInventoryCommand
        (
            this,
            UIManager.selectedSlot,         // Slot of selected weapons
            weaponManager,                  // Weapon game manager class
            UIManager                       // UI manager class
        );


        addToInvCommandUI = new AddToInventoryCommandUI<IIdentifiable>
        (
            weaponManager,
            UIManager,
            UIManager.slotPrefabPath,               // Path to the prefab (resources)
            null                                    // Is addedWeapon
        );

        switchWeaponCommand = new SwapWeaponCommand 
        (
            weaponManager
        );

        activateWeaponCommand = new ActivateWeaponCommand 
        (
            this,
            weaponManager,
            UIManager
        );

        normalAttackCommand = ScriptableObject.CreateInstance<AttackCommand>();
        normalAttackCommand.Initialize(gameManager, weaponManager);;
    } 

    public void CustomStart() 
    {
        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        input.BindInputToCommand(KeyCode.Tab, openInvCommandUI);
        input.BindInputToCommand(KeyCode.Q, switchWeaponCommand);
        input.BindInputToCommand(KeyCode.Z, normalAttackCommand);
    }

    protected override void Update() 
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
                    weaponManager.weaponsInventoryAsGameObjectList, 
                    inventory,
                    tempWeapon
                );

                // Add weapon to list if weapon is not active
                for (int i = 0; i < weaponManager.allWeapons.Count; i++)
                {
                    var w = weaponManager.allWeapons[i];
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
                    weaponManager.weaponInventoryDictionary,          // Weapon's inventory dictionary
                    tempWeapon.name,                            // Weapon game object name
                    weaponManager.allWeapons,                   // List of Weapon class
                    weaponManager.weaponInventoryEntries              // Serializable class that hold the keys and values of the dictionary
                );

                // Add to UI
                if(addedWeapon is IIdentifiable identifiable) 
                {
                    addToInvCommandUI = new AddToInventoryCommandUI<IIdentifiable>
                    (
                        weaponManager,
                        UIManager,
                        UIManager.slotPrefabPath,             // Path to the prefab (resources)
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

        if(weaponManager.usableWeaponDictionary.Count > 0
        && weaponManager.usableWeaponDictionary.Count <= 2)
        {
            activateWeaponCommand.Execute();
        }

        if(input.keyCommands.Find(k => k.key == KeyCode.Z)?.command == addCommand)
        {
            normalAttackCommand.Execute();
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            // 3RTH STAGE
            eventManager.InvokeEvent("TryConsume", transform);
        }
    }

    protected override void FixedUpdate() 
    {
        var playerStatesCopy = new Dictionary<string, PlayerState>(playerStates);

        foreach(var kvp in playerStatesCopy)
        {
            kvp.Value.StateFixedUpdate(this);
        }
        
        eventManager.InvokeEvent("UpdateScore", 1);
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
                foreach (R_Weapon weapon in weaponManager.allWeapons)
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

    public void SetMoveSpeed(float _moveSpeed)
    {
        movementSpeed = _moveSpeed;
    }

    public void OpenInventoryUI(R_InputHandler input) 
    {
        // Check if the Tab key is pressed
        if (input.keyCommands.Find(k => k.key == KeyCode.Tab)?.command == addCommand) 
        {
            // Check if the inventory is currently open
            if (openInvCommandUI != null && UIManager.UI_weaponInventory.activeInHierarchy) 
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
