using System.Linq;
using UnityEngine;

[System.Serializable]
public class R_Player
{
    // SerializeField
    // [SerializeField] private List<GameObject> weaponInventory = new();
    [SerializeField] private GameObject activeWeapon;
    [SerializeField] private GameObject tempWeapon;
    [SerializeField] private bool inRange;

    // Public
    public GameObject prefab;
    public Transform spawnPoint;
    

    // Private
    private R_InputHandler input; // Key input
    private ICommand addCommand, removeCommand;
    
    // Private readonly
    private readonly WeaponGameManager w_gameManager;
    private readonly Transform transform; // Player's transform
    private readonly float movementSpeed;
 
    // Constructor
    public R_Player(Transform transform, float movementSpeed, WeaponGameManager w_gameManager) 
    {
        this.transform = transform;
        this.movementSpeed = movementSpeed;
        this.w_gameManager = w_gameManager; // Set the instance

        CustomAwake();
    }

    public void CustomAwake() 
    {
        input = new();
    }

    public void CustomStart() 
    {
        addCommand = new AddCommand<GameObject, R_Weapon>
        (
            w_gameManager.weaponInventory, 
            null,
            w_gameManager.allWeapons,
            w_gameManager.dictionary,
            w_gameManager
            // w_gameManager.dictionary.ToDictionary(kvp => kvp.Key, kvp => (R_Weapon)kvp.Value) // Explicit cast to R_Weapon
        );

        removeCommand = new AddCommand<GameObject, R_Weapon>
        (
            w_gameManager.weaponInventory, 
            null,
            w_gameManager.allWeapons,
            w_gameManager.dictionary,
            w_gameManager
            // w_gameManager.dictionary.ToDictionary(kvp => kvp.Key, kvp => (R_Weapon)kvp.Value) // Explicit cast to R_Weapon
        );

        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        
        // Debug.Log($"addCommand initialized: {addCommand != null}");
        // Debug.Log($"removeCommand initialized: {removeCommand != null}");

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
                addCommand = new AddCommand<GameObject, R_Weapon>
                (
                    w_gameManager.weaponInventory, 
                    tempWeapon,
                    w_gameManager.allWeapons,
                    w_gameManager.dictionary,
                    w_gameManager
                    // w_gameManager.dictionary.ToDictionary(kvp => kvp.Key, kvp => (R_Weapon)kvp.Value) // Explicit cast to R_Weapon

                );
                
                addCommand.Execute();
            }
        }

        // Removing the weapon if the player has weapons in their inventory
        if(input.keyCommands.Find(k => k.key == KeyCode.R)?.command == addCommand) 
        {
            if (w_gameManager.weaponInventory.Count > 0) 
            {                
                GameObject weaponToRemove = w_gameManager.weaponInventory[^1];
                removeCommand = new AddCommand<GameObject, R_Weapon>
                (
                    w_gameManager.weaponInventory, 
                    weaponToRemove,
                    w_gameManager.allWeapons,
                    w_gameManager.dictionary,
                    w_gameManager
                    // w_gameManager.dictionary.ToDictionary(kvp => kvp.Key, kvp => (R_Weapon)kvp.Value) // Explicit cast to R_Weapon
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
