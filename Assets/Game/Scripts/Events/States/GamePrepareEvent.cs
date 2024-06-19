namespace XGames.GameName.Events.States
{
    public struct GamePrepareEvent
    {
        public int levelIndex;

        public GamePrepareEvent(int levelIndex)
        {
            this.levelIndex = levelIndex;
        }
    }
}