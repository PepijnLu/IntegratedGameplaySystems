using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // SerializeField
    [SerializeField] private Player player;
    [SerializeField] private R_WeaponsManager weaponGameManager;
    [SerializeField] private R_UIManager UIManager;
    [SerializeField] private SpriteRenderer overlay;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Tilemap _interactablesTileMap; 
    private EventManager eventManager;
    private List<TileBase> customTileBases;
    protected delegate void Vector3IntDelegate(Vector3Int _transform);
    public GameObject playerObj;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Pepijn")
        {
            GameStartUp();
        }

        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            scoreText.text = $"Score: {ScoreData.score}";
        }

        if(player == null) 
        {
            Debug.Log("Player not found from Start");
            return;
        }

        player.CustomStart();
        weaponGameManager.CustomStart();
    }

    private void GameStartUp()
    {
        //Make a new EventManager
        eventManager = new EventManager(_interactablesTileMap);

        //Make an empty event user and set the static eventManager reference for all objects and subjects to use
        EventUser placeholderEventUser = new();
        placeholderEventUser.SetEventManager(eventManager);

        //For each TileBase, subscribe to the consume event
        customTileBases = GetTypeBasesFromAssets();
        foreach (TileBase _base in customTileBases)
        {
            Debug.Log($"{_base.name}");

            if (_base is CustomConsumableTile _customTile)
            {
                _customTile.SetEventManager(eventManager);
                _customTile.SubscribeToEvent();
            }
        }

        eventManager.SubscribeToEvent("SetTileToNull", new Vector3IntDelegate(SetTileToNull));

        //Initialize the necessary classes
        new UIManager(overlay, scoreText);
        new SceneLoader();
        new DecreaseStatsOverTime();

        // Player
        GameObject prefab = Resources.Load<GameObject>("Player"); 
        playerObj = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        player = new
        (
            this,
            weaponGameManager, 
            UIManager, 
            playerObj.transform, 
            10f
        )
        {
            inventory = playerObj.transform.GetChild(0).gameObject,
            usableInventory = playerObj.transform.GetChild(1).gameObject,
            activeWeaponSlot = playerObj.transform.GetChild(2).gameObject
        };

        Camera.main.gameObject.transform.SetParent(playerObj.transform);

        //Invoke start
        eventManager.InvokeAction("Start");
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Pepijn")
        {
            // LAST STAGE
            //Invoke the update method
            eventManager.InvokeAction("Update");
        }
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Pepijn")
        {
            //Invoke the update method
            eventManager.InvokeAction("FixedUpdate");
        }
    }

    private void SetTileToNull(Vector3Int _cellPosition)
    {
        _interactablesTileMap.SetTile(_cellPosition, null);
    }

    //Get all the custom tiles in a list
    private List<TileBase> GetTypeBasesFromAssets()
    {
        return new List<TileBase>(Resources.LoadAll<TileBase>("CustomTiles"));
    }


    //Functionality for menu buttons
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Pepijn");
    }

    public void QuitApplication()
    {
        Application.Quit();
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