using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;
    [SerializeField] private Tilemap _interactablesTileMap; 
    private List<TileBase> customTileBases;
    protected delegate void Vector3IntDelegate(Vector3Int _transform);
    [SerializeField] private SpriteRenderer overlay;
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Pepijn")
        {
            GameStartUp();
        }

        if(SceneManager.GetActiveScene().name == "GameOver")
        {
            scoreText.text = $"Score: {ScoreData.score}";
        }
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
        new Player();
        new DecreaseStatsOverTime();

        //Invoke start
        eventManager.InvokeAction("Start");
    }

    private void Update()
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