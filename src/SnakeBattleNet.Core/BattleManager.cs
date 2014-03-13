using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        private readonly IList<Fighter> fighters;
        private readonly Replay replay;
        private readonly Random random = new Random();

        public BattleManager(IList<Fighter> fighters, Replay replay)
        {
            this.fighters = fighters;
            this.replay = replay;
        }

        public void Fight(int rounds)
        {
            foreach (var fighter in fighters)
                fighter.Grow(9);

            for (var round = 0; round < rounds; round++)
            {
                var skipped = 0;
                foreach (var fighter in Shuffle(fighters))
                {
                    var directions = fighter.PossibleDirections();
                    if (directions.Length == 0)
                    {
                        skipped++;
                        continue;
                    }
                    var direction = directions[random.Next(directions.Length)];
                    fighter.BiteMove(fighters, direction);
                }

                foreach (var fighter in fighters)
                    replay.SaveFighter(round, fighter);

                if (skipped == fighters.Count) break;
            }
        }

        /// <summary>
        /// According to this http://stackoverflow.com/questions/1287567/c-is-using-random-and-orderby-a-good-shuffle-algorithm
        /// </summary>
        public IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable)
        {
            var elements = enumerable.ToArray();
            for (var i = elements.Length - 1; i > 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
            yield return elements[0];
        }
    }
}
