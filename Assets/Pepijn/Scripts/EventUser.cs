using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUser
{
    protected static EventManager eventManager;
    virtual public void SetEventManager(EventManager _eventManager)
    {
        if (eventManager == null) eventManager = _eventManager;
    }

    virtual protected void Update() {}
}
