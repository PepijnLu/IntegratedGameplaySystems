using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EventUser
{
    private Dictionary<string, float> statsDict = new()
    {
        ["Health"] = 10,
        ["Thirst"] = 10,
        ["Hunger"] = 10,
    };
    private delegate void ChangeStatDelegate(string _stat, float _amount);

    public Player()
    {
        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);
        eventManager.SubscribeToEvent("ChangeStat", new ChangeStatDelegate(ChangeStat));
    }

    protected override void Update()
    {
        Debug.Log("Player Update");
    }

    protected override void FixedUpdate()
    {
        Debug.Log("Player FixedUpdate");
    }

    private void ChangeStat(string _stat, float _amount)
    {
        statsDict[_stat] += _amount;
        Debug.Log($"Player took {_amount} {_stat} and is now at {statsDict[_stat]} {_stat}");
    }
}

