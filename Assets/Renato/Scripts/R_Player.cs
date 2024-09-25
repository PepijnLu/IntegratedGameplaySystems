using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class R_Player
{
    // SerializeField
    [SerializeField] private List<GameObject> weaponInventory = new();
    [SerializeField] private GameObject tempWeapon;
    [SerializeField] private bool inRange;

    // Public
    public GameObject prefab;
    public Transform spawnPoint;
    

    // Private
    private R_InputHandler input; // Key input
    private ICommand addCommand, removeCommand;
    // private ICommand command;
    
    
    // Private readonly
    private readonly Transform transform; // Player's transform
    private readonly float movementSpeed;
 
    // Constructor
    public R_Player(Transform transform, float movementSpeed) 
    {
        this.transform = transform;
        this.movementSpeed = movementSpeed;

        CustomAwake();
    }

    public void CustomAwake() 
    {
        input = new();
        addCommand = new AddCommand<GameObject>(input, addCommand, weaponInventory, null);
        removeCommand = new AddCommand<GameObject>(input, removeCommand, weaponInventory, null);

        input.BindInputToCommand(KeyCode.E, addCommand);
        input.BindInputToCommand(KeyCode.R, removeCommand);
        
        Debug.Log($"addCommand initialized: {addCommand != null}");
        Debug.Log($"addCommand initialized: {removeCommand != null}");
    }
    
    public void CustomUpdate() 
    {
        addCommand = input.HandleKeyInput();
        removeCommand = input.HandleKeyInput();

        input.HandleMovement
        (
            new AxisCommand("Horizontal"), 
            new AxisCommand("Vertical"),
            transform,
            movementSpeed
        );

        if(InRangeOfWeapon()) 
        {
            addCommand = new AddCommand<GameObject>(input, addCommand, weaponInventory, tempWeapon);
            addCommand.Execute();
        }

        
        // Removing the weapon if the player has weapons in their inventory
        if (weaponInventory.Count > 0 && removeCommand != null) 
        {
            // Passing the last weapon from the weapon inventory to remove
            GameObject weaponToRemove = weaponInventory[^1];
            removeCommand = new AddCommand<GameObject>(input, removeCommand, weaponInventory, weaponToRemove);
            removeCommand.Undo();
        }
    }

        // if(inRange) 
        // {
        //     // Execute the commmand to add the weapon
        //     // Need to find a way on how to pass in the Weapon type as the inventory and as the weapon
        //     // The inventory cant consist of Weapon since that class is not attached to a weapon
        //     // So the inventory should be a list of GameObjects
        //     // But if I pass in only the GameObjects, I can't determine what type of weapon it is
        //     // And if I can't do that then there are also no attacks or whatsover linked with that weapon
        //     // Since its only a GameObject without the Weapon class
        //     // addWeaponCommand = new(input, command, weaponInventory, tempWeapon);
        // }

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
                // Debug.Log($"Made contact with the {tempWeapon.name}");
                inRange = true;

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
