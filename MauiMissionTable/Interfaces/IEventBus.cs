namespace MauiMissionTable.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T evt) where T : IEvent;
        void Subscribe<T>(Action<T> handler) where T : IEvent;
    }
}
