using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : EventUser
{
    public Subject()
    {
        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);
    }

    protected override void Update()
    {
        //Check if condition is met
        eventManager.InvokeEvent("ChangeStat", "Health", -1f);
        eventManager.InvokeEvent("ChangeStat", "Thirst", -1f);
    }
}
