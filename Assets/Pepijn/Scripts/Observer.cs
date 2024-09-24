using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Observer : EventUser
{
    public Observer()
    {
        eventManager.SubscribeToEvent("Update", Update);
        eventManager.SubscribeToEvent("TestEvent", () => Debug.Log("test event activated"));
    }

    protected override void Update()
    {
        
    }
}

