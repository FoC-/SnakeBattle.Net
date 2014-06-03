namespace SnakeBattleNet.Core.Replay
{
    public abstract class GameEvent
    {
        public string Name
        {
            get { return GetType().Name; }
        }
    }
}