using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : EventUser
{
    public Dictionary<string, PlayerState> playerStateTypes = new Dictionary<string, PlayerState>(); 
    private Player player;
    private delegate void ChangeStateDelegate(string _state, string _playerState);
    public StateMachine(Player _player)
    {
        player = _player;
        InitializeStates<PlayerState>();
        playerStateTypes = PlayerState.GetPlayerStates();

        eventManager.SubscribeToEvent("ChangeState", new ChangeStateDelegate(ChangeState));
    }

    //Initialize all subclasses that derive of a base class with type T
    private void InitializeStates<T>()
    {
        var allSubTypes = System.Reflection.Assembly.GetAssembly(GetType()).GetTypes()
            .Where(typeToCheck => 
            {
                while(typeToCheck.BaseType != null)
                {
                    if(typeToCheck.BaseType == typeof(T))
                        return true;
                    typeToCheck = typeToCheck.BaseType;
                }
                return false;
            }).ToList();
        foreach(var subType in allSubTypes)
        {
            Debug.Log("Activator: " + Activator.CreateInstance(subType));
        }
    }

    //Set the type of an object to a new tpye
    public void ChangeState(string _stateToChange, string _newState)
    {      
        Debug.Log($"State to change: {_stateToChange} to {_newState}"); 
        player.playerStates[_stateToChange].OnStateExit(player);
        Debug.Log($"State: Previous state was {player.playerStates[_stateToChange]}");
        player.playerStates[_stateToChange] = playerStateTypes[_newState];
        player.playerStates[_stateToChange].OnStateEnter(player);
        Debug.Log($"State: New state is {player.playerStates[_stateToChange]}");
    }
}