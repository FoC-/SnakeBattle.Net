using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Replay
    {
        private readonly Dictionary<string, int> idMapUniqueToShort;

        public string Id { get; private set; }
        public LinkedList<ReplayEvent> Events { get; private set; }

        public Replay( )
        {
            idMapUniqueToShort = new Dictionary<string, int>();
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            Events = new LinkedList<ReplayEvent>();
        }

        public int GetShortId(string longId)
        {
            int id;
            if (!idMapUniqueToShort.TryGetValue(longId, out id))
            {
                id = idMapUniqueToShort.Max(p => p.Value);
                idMapUniqueToShort.Add(longId, id);
            }
            return id;
        }

        public void AddEvent(ReplayEvent replayEvent)
        {
            Events.AddLast(replayEvent);
        }
    }
}