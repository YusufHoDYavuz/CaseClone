namespace XGames.GameName.EventSystem
{
    public delegate void EventListener<in TEvent>(object sender, TEvent @event);
}