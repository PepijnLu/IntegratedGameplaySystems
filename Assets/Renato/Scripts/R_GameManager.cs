using System.Collections.Generic;
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
    private GameObject playerObj;

    // Private Readonly

    void Awake() 
    {
        // Player
        playerObj = Instantiate(player.prefab, player.spawnPoint.position, player.spawnPoint.rotation);        
        player = new(playerObj.transform, 10f, weaponGameManager, r_UIManager);

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
        // r_UIManager.Select(weaponGameManager.weaponInvDict, weaponGameManager);
    }
}

[System.Serializable]

public class R_WeaponsManager 
{
    [Header("Game Management")]
    [SerializeField] public List<R_Weapon> allWeapons;
    [SerializeField] public List<Transform> spawnPoints;

    [Header("Weapons Inventory")]
    [SerializeField] public List<GameObject> weaponInventory = new();
    [SerializeField] public List<SerializableDictionary<R_Weapon>> inventoryEntries = new();
    public Dictionary<string, R_Weapon> weaponInvDictionary = new();

    [Header("Usable Weapons Inventory")]
    [SerializeField] public List<R_Weapon> usableWeapons = new();
    [SerializeField] public List<SerializableDictionary<R_Weapon>> usableEntries = new();
    public Dictionary<string, R_Weapon> usableWeapnDictionary = new();
    
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
            weapon.Instantiate(spawnPoint.transform);
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
    [SerializeField] public GameObject weaponInventoryUI; // UI inventory
    [SerializeField] public List<SerializableDictionary<GameObject>> slotDictionaryEntry = new(); // Class entry to store the values from the dictionary
    public Dictionary<string, GameObject> slotDictionaryUI = new(); // Dictionary to store the UI slots
    [SerializeField] public List<GameObject> slotListUI = new(); // List to store UI slots in (need it in order to add it into the dictionary)

    [Header("Usable Weapons Inventory")]
    [SerializeField] public GameObject usableInventoryUI; // Usable UI inventory
    [SerializeField] public List<GameObject> usableWeaponsUI = new();
    [SerializeField] public List<R_Weapon> selectedSlot = new();
    
    [HideInInspector] public GameObject slotPrefab; // This is just a placeholder for the slotPrefabPath
    [HideInInspector] public string slotPrefabPath = "InventorySlot"; // Path to the UI slot to instantiate
}

/// <summary>
/// This class is meant to store the keys and values from a dictionary
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class SerializableDictionary<T>
{
    public string key;
    public T value;
}