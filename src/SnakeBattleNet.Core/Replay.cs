using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Replay
    {
        public string Id { get; private set; }
        public IDictionary<Position, Content> BattleField { get; set; }
        public IDictionary<int, IDictionary<string, IEnumerable<Move>>> Frames { get; set; }

        public Replay()
        {
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            Frames = new Dictionary<int, IDictionary<string, IEnumerable<Move>>>();
        }

        public void SaveFighter(int round, Fighter fighter)
        {
            IDictionary<string, IEnumerable<Move>> replayEvents;
            if (!Frames.TryGetValue(round, out replayEvents))
            {
                replayEvents = new Dictionary<string, IEnumerable<Move>>();
                Frames.Add(round, replayEvents);
            }
            var cells = fighter.BodyParts.Select(x => new Move { X = x.X, Y = x.Y, Content = Content.Body, Direction = x.Direction }).ToArray();
            cells[cells.Length - 1].Content = Content.Tail;
            cells[0].Content = Content.Head;
            replayEvents.Add(fighter.Id, cells);
        }
    }
}