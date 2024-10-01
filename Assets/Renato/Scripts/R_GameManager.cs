using System.Collections.Generic;
using UnityEngine;

public class R_GameManager : MonoBehaviour
{
    // SerializeField
    [SerializeField] private R_Player player;
    [SerializeField] private WeaponGameManager weaponGameManager;
    [SerializeField] private R_UIManager uIManager;

    // Public

    // Protected

    // Private
    private GameObject playerObj;

    // Private Readonly

    void Awake() 
    {
        // Player
        playerObj = Instantiate(player.prefab, player.spawnPoint.position, player.spawnPoint.rotation);        
        player = new(playerObj.transform, 10f, weaponGameManager);

        // UI
        uIManager = new();
    }

    void Start()
    {
        player.CustomStart();
        weaponGameManager.InstantiateWeapons();
    }

    void Update()
    {
        player.CustomUpdate();
    }
}

[System.Serializable]
public class WeaponGameManager 
{
    public List<R_Weapon> allWeapons;
    public List<Transform> spawnPoints;
    
    // For the player
    public List<GameObject> weaponInventory = new();
    public Dictionary<string, IIdentifiable> dictionary = new();
    
    

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