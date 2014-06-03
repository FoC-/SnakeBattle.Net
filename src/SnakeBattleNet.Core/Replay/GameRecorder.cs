using System.Collections.Generic;
using System.Linq;

namespace SnakeBattleNet.Core.Replay
{
    public class GameRecorder
    {
        public ICollection<IList<GameEvent>> Frames { get; private set; }

        public GameRecorder()
        {
            Frames = new List<IList<GameEvent>>();
        }

        public void StartNewFrame()
        {
            Frames.Add(new List<GameEvent>());
        }

        public void FrameAdd(GameEvent e)
        {
            Frames.Last().Add(e);
        }
    }
}
