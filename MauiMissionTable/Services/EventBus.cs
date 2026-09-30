using MauiMissionTable.Interfaces;

namespace MauiMissionTable.Services
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = [];

        public void Publish<T>(T evt) where T : IEvent
        {
            if (_handlers.TryGetValue(typeof(T), out var list))
            {
                foreach (var handler in list.Cast<Action<T>>())
                {
                    try
                    {
                        handler(evt);
                    }
                    catch (Exception) { /* ignore */ }
                }
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            if (handler is null)
                return;

            if (!_handlers.TryGetValue(typeof(T), out var list))
            {
                list = [];
                _handlers[typeof(T)] = list;
            }

            list.Add(handler);
        }
    }
}
