using System.Collections.Generic;
using UnityEngine;

public class PlayerState : EventUser
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
    public virtual void OnStateEnter(Player _obj) {}

    //Logic for exiting a state
    public virtual void OnStateExit(Player _obj) {}

    //State update function
    public virtual void StateUpdate(Player _obj) {}

    //State fixed update function
    public virtual void StateFixedUpdate(Player _obj) {}
}


//All the different states that correspond to how high a stat is
public class NormalHungerState : PlayerState
{
    public override void StateFixedUpdate(Player _obj)
    {
        eventManager.InvokeEvent("ChangeStat", "Health", 0.015f, true);
    }
}

public class LowHungerState : PlayerState
{

}

public class ZeroHungerState : PlayerState
{
    public override void StateFixedUpdate(Player _obj)
    {
        eventManager.InvokeEvent("ChangeStat", "Health", -0.015f, true);
    }
}

public class NormalThirstState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        eventManager.InvokeEvent("ChangeOverlayOpacity", 0f);
    }
}
public class LowThirstState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        eventManager.InvokeEvent("ChangeOverlayOpacity", 0.68f);
    }
}

public class ZeroThirstState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        eventManager.InvokeEvent("ChangeOverlayOpacity", 0.98f);
    }
}

public class NormalHealthState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        _obj.SetMoveSpeed(10f);
    }
}

public class LowHealthState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        _obj.SetMoveSpeed(5f);
    }
}

public class ZeroHealthState : PlayerState
{
    public override void OnStateEnter(Player _obj)
    {
        eventManager.InvokeEvent("LoadScene", "GameOver");
    }
}
