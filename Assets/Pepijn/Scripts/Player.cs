using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EventUser
{
    private PlayerStats playerStats;
    private delegate void ChangeStatDelegate(string _stat, float _amount);

    public Player()
    {
        playerStats = new PlayerStats(this);

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
        playerStats.statsDict[_stat]["Current" + _stat] += _amount;
        //Debug.Log($"Player took {_amount} {_stat} and is now at {playerStats.statsDict[_stat]["Current" + _stat]} {_stat}");
        eventManager.InvokeEvent("CheckStat", _stat);
    }
}

