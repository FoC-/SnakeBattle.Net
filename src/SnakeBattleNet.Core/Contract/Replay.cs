using System;
using System.Collections.Generic;

namespace SnakeBattleNet.Core.Contract
{
    public class Replay
    {
        public string Id { get; private set; }
        public IDictionary<string, int> IdMapUniqueToShort { get; set; }
        public IDictionary<Position, Content> BattleField { get; set; }
        public IDictionary<int, ICollection<ReplayEvent>> Frames { get; set; }

        public Replay()
        {
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }
    }
}