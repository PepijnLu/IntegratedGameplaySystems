using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EventManager eventManager;
    void Start()
    {
        eventManager = new EventManager();
        eventManager.InvokeEvent("Start");

        //Make an empty event user and set the static eventManager reference for all objects and subjects to use
        EventUser placeholderEventUser = new();
        placeholderEventUser.SetEventManager(eventManager);

        new Observer();
        new Subject();
    }

    void Update()
    {
        eventManager.InvokeEvent("Update");
    }

}