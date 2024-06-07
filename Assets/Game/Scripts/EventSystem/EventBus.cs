using System.Collections.Generic;

namespace XGames.GameName.EventSystem
{
    public static class EventBus<TEvent>
    {
        private static readonly List<EventListener<TEvent>> Listeners = new();

        public static void AddListener(EventListener<TEvent> eventListener)
        {
            Listeners.Add(eventListener);
        }
        
        public static void RemoveListener(EventListener<TEvent> eventListener)
        {
            Listeners.Remove(eventListener);
        }
        
        public static void Emit(object sender, TEvent @event)
        {
            for (var i = Listeners.Count - 1; i >= 0; i--)
            {
                Listeners[i]?.Invoke(sender, @event);
            }
        }
    }
}

