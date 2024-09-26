using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EventUser
{
    private Player player;
    private delegate void CheckStatDelegate(string _stat);

    public StatState thirstState;

    public PlayerStats(Player _player)
    {
        player = _player;
        //eventManager.SubscribeToEvent("CheckStat", new CheckStatDelegate(CheckStat));
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
        { "LowThirst", 3f }
    }
    }
    };

    // private StatState CheckStat(string _stat)
    // {
    //     Debug.Log("Check stat: " + _stat);
        
    //     float statAmount = statsDict[_stat]["Current" + _stat];
    //     if (statAmount == statsDict[_stat]["Max" + _stat]) return
    //     //statsDict[_stat]["Current" + _stat];
    // }


}
