using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Replay
    {
        public int RandomSeed { get; private set; }
        public BattleField BattleField { get; set; }
        public IDictionary<int, IDictionary<string, IEnumerable<Cell<Content>>>> Frames { get; set; }

        public Replay(int randomSeed)
        {
            RandomSeed = randomSeed;
            Frames = new Dictionary<int, IDictionary<string, IEnumerable<Cell<Content>>>>();
        }

        public void SaveFighter(int round, Fighter fighter)
        {
            IDictionary<string, IEnumerable<Cell<Content>>> replayEvents;
            if (!Frames.TryGetValue(round, out replayEvents))
            {
                replayEvents = new Dictionary<string, IEnumerable<Cell<Content>>>();
                Frames.Add(round, replayEvents);
            }

            var cells = fighter.Body.Select(x => new Cell<Content> { X = x.X, Y = x.Y, Content = Content.Body }).ToList();

            if (fighter.Head != null)
                cells.Add(new Cell<Content> { X = fighter.Head.X, Y = fighter.Head.Y, Content = Content.Head });

            if (fighter.Tail != null)
                cells.Add(new Cell<Content> { X = fighter.Tail.X, Y = fighter.Tail.Y, Content = Content.Tail });

            replayEvents.Add(fighter.Id, cells);
        }
    }
}