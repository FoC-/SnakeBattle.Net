using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        private readonly IList<Fighter> fighters;
        private readonly Replay replay;
        private readonly FieldComparer fieldComparer;
        private readonly Random random = new Random();

        public BattleManager(IList<Fighter> fighters, Replay replay, FieldComparer fieldComparer)
        {
            this.fighters = fighters;
            this.replay = replay;
            this.fieldComparer = fieldComparer;
        }

        public void Fight(int rounds)
        {
            foreach (var fighter in fighters)
                fighter.Grow(fighter.Head.Direction, 9);

            for (var round = 0; round < rounds; round++)
            {
                var skipped = 0;
                foreach (var fighter in Shuffle(fighters))
                {
                    var possibleDirections = fieldComparer.PossibleDirections(fighter.Head, fighters);
                    var directions = fieldComparer.DecidedDirections(fighter, possibleDirections);
                    if (directions.Length == 0)
                    {
                        skipped++;
                        continue;
                    }
                    var direction = directions[random.Next(directions.Length)];
                    TryBite(fighter, direction);
                }

                foreach (var fighter in fighters)
                    replay.SaveFighter(round, fighter);

                if (skipped == fighters.Count) break;
            }
        }

        private void TryBite(Fighter biting, Direction direction)
        {
            var newHead = Directed.ToDirection(biting.Head, direction);
            var bitten = fighters.FirstOrDefault(f => f.Tail.X == newHead.X && f.Tail.Y == newHead.Y);
            if (bitten == null)
            {
                biting.Grow(direction);
                biting.CutTail();
            }
            else
            {
                bitten.CutTail();
                biting.Grow(direction);
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
