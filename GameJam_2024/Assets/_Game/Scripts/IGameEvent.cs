public interface IGameEvent
{
    public void RegisterEvent(EventType type);
    public void DeregisterEvent(EventType type);
    
}