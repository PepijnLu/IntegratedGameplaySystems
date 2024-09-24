using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : EventUser
{
    public Subject()
    {
        eventManager.SubscribeToEvent("Update", Update);
    }

    protected override void Update()
    {
        //Check if condition is met
        eventManager.InvokeEvent("TestEvent");
    }
}
