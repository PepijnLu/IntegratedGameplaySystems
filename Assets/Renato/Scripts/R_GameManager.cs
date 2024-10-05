using System.Collections.Generic;
using UnityEngine;

public class R_GameManager : MonoBehaviour
{
    // SerializeField
    [SerializeField] private R_Player Player;
    [SerializeField] private WeaponsGameManager weaponsGameManager;
    [SerializeField] private R_UIManager uIManager;

    // Public

    // Protected

    // Private
    private GameObject player;

    // Private Readonly

    void Awake() 
    {
        player = Instantiate(Player.prefab, Player.spawnPoint.position, Player.spawnPoint.rotation);
        Camera.main.gameObject.transform.SetParent(player.transform);
        
        // Instantiate R_Player with the player's Transform
        Player = new(player.transform, 10f);
        uIManager = new();
    }

    void Start()
    {
        weaponsGameManager.ManageWeapons();
    }

    void Update()
    {
        Player.CustomUpdate();
    }
}

[System.Serializable]
public class WeaponsGameManager 
{
    public List<R_Weapon> weapons;
    public List<Transform> spawnPoints;

    public void ManageWeapons() 
    {
        if(weapons == null || spawnPoints == null 
        || weapons.Count == 0 || spawnPoints.Count == 0) return;

        // Shuffle the weapons and spawn points lists to randomize them
        ShuffleList(weapons);
        ShuffleList(spawnPoints);

        // Determine the number of weapons to spawn
        int weaponsToSpawn = Mathf.Min(weapons.Count, spawnPoints.Count);

        // Spawn each weapon at a random spawn point
        for (int i = 0; i < weaponsToSpawn; i++)
        {
            R_Weapon weapon = weapons[i];
            GameObject spawnPoint = spawnPoints[i].gameObject;
            weapon.Spawn(spawnPoint.transform);
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