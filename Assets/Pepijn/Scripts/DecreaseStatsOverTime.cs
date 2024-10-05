using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseStatsOverTime : EventUser
{
    public DecreaseStatsOverTime()
    {
        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);
    }

    protected override void Update()
    {

    }

    protected override void FixedUpdate()
    {
        //Check if condition is met
        //eventManager.InvokeEvent("ChangeStat", "Health", -0.005f, true);
        eventManager.InvokeEvent("ChangeStat", "Thirst", -0.015f, true);
        eventManager.InvokeEvent("ChangeStat", "Hunger", -0.015f, true);
    }
}
