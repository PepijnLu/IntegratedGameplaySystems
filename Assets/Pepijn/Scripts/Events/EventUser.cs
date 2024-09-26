public class EventUser
{
    protected static EventManager eventManager;
    protected delegate void StringFloatDelegate(string _string, float _float);
    virtual public void SetEventManager(EventManager _eventManager)
    {
        //Set the event manager and subscribe to the basic methods
        if (eventManager == null) 
        {
            eventManager = _eventManager;
        }
    }

    virtual protected void Update() {}
    virtual protected void FixedUpdate() {}
}
