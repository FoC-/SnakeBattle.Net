using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeBattleNet.ReplayRecorder.Contracts
{
    public class Replay
    {
        public string Id { get; private set; }
        public int FieldWidth { get; private set; }
        public int FieldHeight { get; private set; }
        public int RandomSeed { get; private set; }
        public Dictionary<string, int> UniqueToShortIdMap { get; private set; }
        public LinkedList<ReplayEvent> events { get; private set; }

        internal Replay()
        {
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            events = new LinkedList<ReplayEvent>();
        }

        internal void SetFieldWidth(int fieldWidth)
        {
            FieldWidth = fieldWidth;
        }

        internal void SetFieldHeight(int fieldHeight)
        {
            FieldHeight = fieldHeight;
        }

        internal void SetRandomSeed(int randomSeed)
        {
            RandomSeed = randomSeed;
        }

        internal void SetUniqueToShortIdMap(IEnumerable<string> ids)
        {
            var uids = new HashSet<string>(ids);
            int counter = 1;
            UniqueToShortIdMap = uids.ToDictionary(key => key, value => counter++);
        }

        internal void AddEvent(ReplayEvent replayEvent)
        {
            events.AddLast(replayEvent);
        }
    }
}