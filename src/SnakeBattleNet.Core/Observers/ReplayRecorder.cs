using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Observers
{
    public class ReplayRecorder : IObserver
    {
        private readonly Dictionary<string, int> idMapUniqueToShort;
        private readonly IDictionary<int, ICollection<ReplayEvent>> frames;
        private int frameIndex;
        private readonly Replay replay;

        public ReplayRecorder()
        {
            idMapUniqueToShort = new Dictionary<string, int>();
            frames = new Dictionary<int, ICollection<ReplayEvent>>();
            replay = new Replay();
        }

        public int GetShortId(string longId)
        {
            int id;
            if (!idMapUniqueToShort.TryGetValue(longId, out id))
            {
                var max = idMapUniqueToShort.Max(p => p.Value);
                id = max++;
                idMapUniqueToShort.Add(longId, id);
            }
            return id;
        }

        public void SetBattlefield(View<Content> battleField)
        {
            replay.BattleField = battleField.ToDictionary(k => k.Key, v => v.Value);
        }

        public void SetFrameIndex(int index)
        {
            frameIndex = index;
        }

        public void Notify(ReplayEvent replayEvent)
        {
            ICollection<ReplayEvent> replayEvents;
            if (!frames.TryGetValue(frameIndex, out replayEvents))
            {
                replayEvents = new List<ReplayEvent>();
                frames.Add(frameIndex, replayEvents);
            }
            replayEvents.Add(replayEvent);
        }

        public Replay Replay()
        {
            replay.Frames = frames;
            return replay;
        }
    }
}