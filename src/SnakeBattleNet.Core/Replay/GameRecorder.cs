using System.Collections.Generic;

namespace SnakeBattleNet.Core.Replay
{
    public class GameRecorder
    {
        public ICollection<GameEvent> Events { get; private set; }

        public GameRecorder()
        {
            Events = new List<GameEvent>();
        }

        public void Add(GameEvent e)
        {
            Events.Add(e);
        }
    }
}
