using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected static Dictionary<string, PlayerState> playerStates = new Dictionary<string, PlayerState>();
    public PlayerState()
    {
        //Add the instantiated class to the dictionary if it's not in it yet
        if(!playerStates.ContainsKey(GetType().Name))
        {
            playerStates.Add(GetType().Name, this);
            Debug.Log($"Dictionary: {GetType().Name} added");
        }
    }

    public static Dictionary<string, PlayerState> GetPlayerStates()
    {
        return playerStates;
    }

    //Logic for entering a state
    public void OnStateEnter(Player _obj)
    {

    }

    //Logic for exiting a state
    public void OnStateExit(Player _obj)
    {

    }
}

public class MaxHungerState : PlayerState
{

}

public class NormalHungerState : PlayerState
{

}

public class LowHungerState : PlayerState
{

}

public class ZeroHungerState : PlayerState
{

}

public class MaxThirstState : PlayerState
{

}

public class NormalThirstState : PlayerState
{

}
public class LowThirstState : PlayerState
{

}

public class ZeroThirstState : PlayerState
{

}

public class MaxHealthState : PlayerState
{

}
public class NormalHealthState : PlayerState
{

}

public class LowHealthState : PlayerState
{

}

public class ZeroHealthState : PlayerState
{

}
