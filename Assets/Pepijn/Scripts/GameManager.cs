using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;
    private UIManager uiManager;
    private void Start()
    {
        //Make a new EventManager
        eventManager = new EventManager();

        //Make an empty event user and set the static eventManager reference for all objects and subjects to use
        EventUser placeholderEventUser = new();
        placeholderEventUser.SetEventManager(eventManager);

        new Player();
        new DecreaseStatsOverTime();
        uiManager = new();

        //Invoke start
        eventManager.InvokeAction("Start");
    }

    private void Update()
    {
        //Invoke the update method
        eventManager.InvokeAction("Update");
    }
    
    private void FixedUpdate()
    {
        //Invoke the update method
        eventManager.InvokeAction("FixedUpdate");
    }

}