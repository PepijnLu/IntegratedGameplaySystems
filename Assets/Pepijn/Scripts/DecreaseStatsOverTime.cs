public class DecreaseStatsOverTime : EventUser
{
    public DecreaseStatsOverTime()
    {
        eventManager.SubscribeToAction("Update", Update);
        eventManager.SubscribeToAction("FixedUpdate", FixedUpdate);
    }

    protected override void FixedUpdate()
    {
        eventManager.InvokeEvent("ChangeStat", "Thirst", -0.0075f / 2, true);
        eventManager.InvokeEvent("ChangeStat", "Hunger", -0.0075f / 2, true);
    }
}
