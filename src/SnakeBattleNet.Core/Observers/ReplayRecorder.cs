using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Observers
{
    public class ReplayRecorder : IObserver
    {
        public string Id { get; private set; }

        private readonly Dictionary<string, int> idMapUniqueToShort;
        private readonly IDictionary<int, ICollection<ReplayEvent>> frames;
        private int frameIndex;

        public ReplayRecorder()
        {
            idMapUniqueToShort = new Dictionary<string, int>();
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            frames = new Dictionary<int, ICollection<ReplayEvent>>();
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

        public IDictionary<int, ICollection<ReplayEvent>> Replay()
        {
            return frames;
        }
    }
}