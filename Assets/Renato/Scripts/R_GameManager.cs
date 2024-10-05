using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class R_GameManager : MonoBehaviour
{
    // SerializeField
    [SerializeField] private R_Player player;
    [SerializeField] private R_WeaponsManager weaponGameManager;
    [SerializeField] private R_UIManager r_UIManager;

    // Public

    // Protected

    // Private
    public GameObject playerObj;

    // Private Readonly

    void Awake() 
    {
        // Player
        playerObj = Instantiate(player.prefab, player.spawnPoint.position, player.spawnPoint.rotation);
        player = new(playerObj.transform, 10f, weaponGameManager, r_UIManager)
        {
            inventory = playerObj.transform.GetChild(0).gameObject,
            usableInventory = playerObj.transform.GetChild(1).gameObject,
            activeWeaponSlot = playerObj.transform.GetChild(2).gameObject
        };

        // UI
    }

    void Start()
    {
        player.CustomStart();
        weaponGameManager.CustomStart();
    }

    void Update()
    {
        player.CustomUpdate();
    }
}

[System.Serializable]

public class R_WeaponsManager 
{
    [Header("Game Management")]
    [SerializeField] public List<R_Weapon> allWeapons;
    [SerializeField] public List<Transform> spawnPoints;

    [Header("Weapons Inventory")]
    [SerializeField] public List<GameObject> weaponsInventoryAsGameObjectList = new();
    [SerializeField] public List<SerializableDictionary<string, R_Weapon>> weaponInventoryEntries = new();
    public Dictionary<string, R_Weapon> weaponInventoryDictionary = new();

    [Header("Usable Weapons Inventory")]
    // [SerializeField] public List<R_Weapon> usableScriptableWeapons = new();
    [SerializeField] public List<GameObject> usableWeaponsAsGameObjectList = new();
    [SerializeField] public List<SerializableDictionary<string, R_Weapon>> usableWeaponInventoryEntries = new();
    public Dictionary<string, R_Weapon> usableWeaponDictionary = new();
    // [SerializeField] public List<R_Weapon> activeWeapon = new();
    [SerializeField] public R_Weapon activeWeapon;
    [SerializeField] public GameObject activeWeaponGameObject;
    
    [SerializeField] public List<SerializableDictionary<GameObject, R_Weapon>> activeWeaponEntry = new();
    
    public void CustomStart() 
    {
        InstantiateWeapons();
    }

    // Instantiates weapons at the start of the game
    public void InstantiateWeapons() 
    {
        if(allWeapons == null || spawnPoints == null 
        || allWeapons.Count == 0 || spawnPoints.Count == 0) return;

        // Shuffle the weapons and spawn points lists to randomize them
        ShuffleList(allWeapons);
        ShuffleList(spawnPoints);

        // Determine the number of weapons to spawn
        int weaponsToSpawn = Mathf.Min(allWeapons.Count, spawnPoints.Count);

        // Spawn each weapon at a random spawn point
        for (int i = 0; i < weaponsToSpawn; i++)
        {
            R_Weapon weapon = allWeapons[i];
            GameObject spawnPoint = spawnPoints[i].gameObject;
            weapon.InstantiateAtStart(spawnPoint.transform);
        }
    }

    // Shuffles the list
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);

            // The line above is actually the code below

            // T temp = list[i];
            // list[i] = list[randomIndex];
            // list[randomIndex] = temp;
        }
    }
}

[System.Serializable]
public class R_UIManager
{
    [Header("Weapons Inventory")]
    [SerializeField] public GameObject UI_weaponInventory; // UI inventory
    [SerializeField] public List<SerializableDictionary<string, GameObject>> slotDictionaryEntry = new(); // Class entry to store the values from the dictionary
    public Dictionary<string, GameObject> UI_slotDictionary = new(); // Dictionary to store the UI slots
    [SerializeField] public List<GameObject> UI_slotList = new(); // List to store UI slots in (need it in order to add it into the dictionary)

    [Header("Usable Weapons Inventory")]
    [SerializeField] public GameObject UI_usableWeaponsInventory; // Usable UI inventory
    [SerializeField] public List<GameObject> UI_UsableWeaponsList = new();
    [SerializeField] public List<R_Weapon> selectedSlot = new();
        
    [HideInInspector] public GameObject slotPrefab; // This is just a placeholder for the slotPrefabPath
    [HideInInspector] public string slotPrefabPath = "InventorySlot"; // Path to the UI slot to instantiate
}

/// <summary>
/// This class is meant to store the keys and values from a dictionary
/// </summary>
[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
    public TKey key;
    public TValue value;
}