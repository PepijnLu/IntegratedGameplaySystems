using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EventUser
{
    private PlayerStats playerStats;
    public StateMachine stateMachine;
    public Dictionary<string, PlayerState> playerStates;
    public Player()
    {
        //Give the player stats and a statemachine
        playerStats = new();
        stateMachine = new(this);

        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);

        //Set the beginning survival states
        playerStates = new()
        {
            ["HungerState"] = stateMachine.playerStateTypes["MaxHungerState"],
            ["HealthState"] = stateMachine.playerStateTypes["MaxHealthState"],
            ["ThirstState"] = stateMachine.playerStateTypes["MaxThirstState"],
        };
    }

    protected override void Update()
    {
        //Test inputs for healing / eating / drinking
        if(Input.GetKeyDown(KeyCode.Q))
        {
            eventManager.InvokeEvent("ChangeStat", "Health", 1f, true);
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            eventManager.InvokeEvent("ChangeStat", "Hunger", 1f, true);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            eventManager.InvokeEvent("ChangeStat", "Thirst", 1f, true);
        }
    }

    protected override void FixedUpdate()
    {
        Debug.Log("Player FixedUpdate");
    }
}

