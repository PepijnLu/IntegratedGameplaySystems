using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private Dictionary<string, Action> actions = new();
    private List<string> events = new()
    {
        "Start",
        "Update",
        "TestEvent"
    };

    public EventManager()
    {
        FillDictionary();
    }

    private void FillDictionary()
    {
        foreach(string str in events)
        {
            actions.Add(str, () => {}); 
        }
    }
    public void InvokeEvent(string myEvent)
    {
        actions[myEvent]?.Invoke();
    }

    public void SubscribeToEvent(string _myEvent, Action method)
    {
        actions[_myEvent] += method;
    }

}
