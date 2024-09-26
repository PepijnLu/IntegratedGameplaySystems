using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : EventUser
{
    private Player player;
    private delegate void ChangeStatDelegate(string _stat, float _amount, bool justIncrement);
    private delegate void CheckStatDelegate(string _stat, float _newAmount);

    public PlayerState healthState, hungerState, thirstState;

    public PlayerStats()
    {
        eventManager.SubscribeToEvent("ChangeStat", new ChangeStatDelegate(ChangeStat));
        eventManager.SubscribeToEvent("OnStatChanged", new StringFloatDelegate(CheckStat));

        eventManager.InvokeEvent("ChangeStat", "Health", statsDict["Health"]["MaxHealth"], false);
        eventManager.InvokeEvent("ChangeStat", "Thirst", statsDict["Thirst"]["MaxThirst"], false);
        eventManager.InvokeEvent("ChangeStat", "Hunger", statsDict["Hunger"]["MaxHunger"], false);
    }

    public Dictionary<string, Dictionary<string, float>> statsDict = new()
    {
        { "Health", new Dictionary<string, float>()
        {
            { "CurrentHealth", 10f },
            { "MaxHealth", 10f },
            { "LowHealth", 3f },
        }
        },

        { "Thirst", new Dictionary<string, float>()
        {
            { "CurrentThirst", 10f },
            { "MaxThirst", 10f },
            { "LowThirst", 3f }
        }
    },
    { "Hunger", new Dictionary<string, float>()
    {
        { "CurrentHunger", 10f },
        { "MaxHunger", 10f },
        { "LowHunger", 3f }
    }
    }
    };

    //Change a stat
    private void ChangeStat(string _stat, float _amount, bool justIncrement)
    {
        if (justIncrement) statsDict[_stat][$"Current{_stat}"] += _amount;
        else statsDict[_stat][$"Current{_stat}"] = _amount;

        eventManager.InvokeEvent("OnStatChanged", _stat, statsDict[_stat][$"Current{_stat}"]);
    }

    //Check the changed stats and invoke a state change event accordingly
    private void CheckStat(string _stat, float _newAmount)
    {
        string _newState;
        
        float statAmount = statsDict[_stat][$"Current{_stat}"];

        if (statAmount <= 0)
        {
            statsDict[_stat][$"Current{_stat}"] = 0;
            _newState = $"Zero{_stat}State";
        }
        else if (statAmount <= statsDict[_stat][$"Low{_stat}"])
        {
            _newState = $"Low{_stat}State";
        }

        else if (statAmount >= statsDict[_stat][$"Max{_stat}"])
        {
            statsDict[_stat][$"Current{_stat}"] = statsDict[_stat][$"Max{_stat}"];
            _newState = $"Max{_stat}State";
        }
        else
        {
            _newState = $"Normal{_stat}State";
        }

        if (_newState != null) eventManager.InvokeEvent("ChangeState", $"{_stat}State", _newState);
    }


}
