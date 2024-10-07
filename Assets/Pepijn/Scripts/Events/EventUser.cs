using UnityEngine;

public class EventUser
{
    protected static EventManager eventManager;
    protected delegate void StringFloatDelegate(string _string, float _float);
    protected delegate void FloatDelegate(float _float);
    protected delegate void StringDelegate(string _string);
    protected delegate void TransformDelegate(Transform _transform);
    virtual public void SetEventManager(EventManager _eventManager)
    {
        //Set the event manager and subscribe to the basic methods
        if (eventManager == null) 
        {
            eventManager = _eventManager;
        }
    }

    virtual public EventManager GetEventManager()
    {
        return eventManager;    
    }

    virtual protected void Update() {}
    virtual protected void FixedUpdate() {}
}
