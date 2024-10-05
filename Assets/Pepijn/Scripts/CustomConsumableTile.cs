using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewCustomTile", menuName = "Tiles/CustomTile")]
public class CustomConsumableTile : Tile, IConsumable
{
    protected delegate void TransformDelegate(Transform _transform);
    private static EventManager eventManager;
    [SerializeField] private float amountToChange;
    [SerializeField] private string statToChange;
    [SerializeField] private int consumesBeforeDelete;
    private Vector3Int tilePosition;
    private Dictionary<Vector3Int, int> tilePositionsConsumesLeft = new();

    public void SetEventManager(EventManager _eventManager)
    {
        if(eventManager == null) eventManager = _eventManager;
    }

    public void SubscribeToEvent()
    {
        Debug.Log("Tile Subscribed To Event");
        eventManager.SubscribeToEvent("TryConsume", new TransformDelegate(TryConsumeTile));
    }
    public void Consume(Vector3Int _cellPosition)
    {
        eventManager.InvokeEvent("ChangeStat", statToChange, amountToChange, true);

        if(tilePositionsConsumesLeft[_cellPosition] == 1)
        {
            tilePositionsConsumesLeft.Remove(_cellPosition);
            eventManager.InvokeEvent("SetTileToNull", _cellPosition);
        }
        else 
        {
            tilePositionsConsumesLeft[_cellPosition]--;
        }
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(!tilePositionsConsumesLeft.ContainsKey(position)) tilePositionsConsumesLeft.Add(position, consumesBeforeDelete);

        Debug.Log($"Custom Tile at {tilePosition}");

        return false; 
    }
    private void TryConsumeTile(Transform _transform)
    {
        // ERROR STARTS HERE
        Vector3Int cellPosition = eventManager.interactableTileMap.WorldToCell(_transform.position);

        if(eventManager.interactableTileMap == null) 
        {
            Debug.LogWarning("No interactable tilemap found");
        }

        if(tilePositionsConsumesLeft.ContainsKey(cellPosition))
        {
            Debug.Log($"Tile consumed at {cellPosition}");
            Consume(cellPosition);
        }
    }
}
