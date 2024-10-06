// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Player : EventUser
// {
//     private PlayerStats playerStats;
//     public StateMachine stateMachine;
//     public Dictionary<string, PlayerState> playerStates;
//     public Player()
//     {
//         //Give the player stats and a statemachine
//         playerStats = new();
//         stateMachine = new(this);

//         eventManager.SubscribeToAction("Update", Update);
//         eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);

//         //Set the beginning survival states
//         playerStates = new()
//         {
//             ["HungerState"] = stateMachine.playerStateTypes["NormalHungerState"],
//             ["HealthState"] = stateMachine.playerStateTypes["NormalHealthState"],
//             ["ThirstState"] = stateMachine.playerStateTypes["NormalThirstState"],
//         };
//     }

//     protected override void Update()
//     {
//         //Try consume with Renato's player transform
//         if(Input.GetKeyDown(KeyCode.V))
//         {
//             // 3RTH STAGE
//             eventManager.InvokeEvent("TryConsume", R_Player.publicTransform);
//         }
//     }

//     protected override void FixedUpdate()
//     {
//         var playerStatesCopy = new Dictionary<string, PlayerState>(playerStates);

//         foreach(var kvp in playerStatesCopy)
//         {
//             kvp.Value.StateUpdate(this);
//         }
        
//         eventManager.InvokeEvent("UpdateScore", 1);
//     }
// }

