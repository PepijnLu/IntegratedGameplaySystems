using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private Dictionary<string, Action> actions = new();
    private Dictionary<string, Delegate> events = new();
    public EventManager() {}

//Actions are empty events
    public void InvokeAction(string myEvent)
    {
        if (actions.ContainsKey(myEvent))
        {
            actions[myEvent]?.Invoke();
        }
    }

    public void SubscribeToAction(string _myEvent, Action method)
    {
        if (actions.ContainsKey(_myEvent))
        {
            actions[_myEvent] += method;
        }
        else
        {
            actions[_myEvent] = method;
        };
    }

//Events are delegates that take parameters
    public void InvokeEvent(string myEvent, params object[] paramaters)
    {
        if (events.ContainsKey(myEvent))
        {
            events[myEvent]?.DynamicInvoke(paramaters);
        }
    }

    public void SubscribeToEvent(string _myEvent, Delegate method)
    {
        if (events.ContainsKey(_myEvent))
        {
            events[_myEvent] = Delegate.Combine(events[_myEvent], method);
        }
        else
        {
            events[_myEvent] = method;
        };
    }
}
