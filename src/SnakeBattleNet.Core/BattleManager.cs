using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Replay;
using SnakeBattleNet.Core.Replay.GameEvents;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        private readonly IList<Fighter> fighters;
        private readonly GameRecorder gameRecorder;
        private readonly FieldComparer fieldComparer;
        private readonly BattleField battleField;
        private readonly Random random;

        public BattleManager(IList<Fighter> fighters, GameRecorder gameRecorder, FieldComparer fieldComparer, BattleField battleField, int randomSeed)
        {
            this.fighters = fighters;
            this.gameRecorder = gameRecorder;
            this.fieldComparer = fieldComparer;
            this.battleField = battleField;
            random = new Random(randomSeed);

            gameRecorder.StartNewFrame();
            gameRecorder.FrameAdd(new GameInit { RandomSeed = randomSeed, BattleField = (BattleField)battleField.Clone() });
            foreach (var fighter in fighters) gameRecorder.FrameAdd(new SnakeGrow { Snake = fighter.Id, NewHeadPosition = fighter.Tail });
        }

        public void Fight(int rounds)
        {
            foreach (var fighter in fighters)
            {
                gameRecorder.StartNewFrame();
                for (var i = 0; i < 9; i++)
                {
                    fighter.Grow(fighter.Tail.Direction);
                    gameRecorder.FrameAdd(new SnakeGrow { Snake = fighter.Id, NewHeadPosition = fighter.Head });
                }
                PutOnBattleField(fighter);
            }

            for (var round = 0; round < rounds; round++)
            {
                gameRecorder.StartNewFrame();
                var skipped = 0;
                foreach (var fighter in Shuffle(fighters))
                {
                    var possibleDirections = fieldComparer.PossibleDirections(fighter, fighters.Except(new[] { fighter }));
                    if (possibleDirections.Length == 0)
                    {
                        skipped++;
                        continue;
                    }
                    var directions = fieldComparer.DecidedDirections(fighter, possibleDirections);
                    var direction = directions.Item2[random.Next(directions.Item2.Length)];
                    TryBite(fighter, direction, directions.Item1);
                }
                if (skipped == fighters.Count) break;
            }
        }

        private void TryBite(Fighter biting, Direction direction, int chip)
        {
            var newHead = Directed.ToDirection(biting.Head, direction);
            var bitten = fighters.FirstOrDefault(f => f.Tail.X == newHead.X && f.Tail.Y == newHead.Y);
            if (bitten == null)
            {
                Grow(biting, direction);
                CutTail(biting);
                gameRecorder.FrameAdd(new SnakeMove { Snake = biting.Id, ChipUsed = chip, NewHeadPosition = newHead });
            }
            else
            {
                CutTail(bitten);
                Grow(biting, direction);
                gameRecorder.FrameAdd(new SnakeBite { Snake = biting.Id, ChipUsed = chip, TargetSnake = bitten.Id });
            }
        }

        private void PutOnBattleField(Fighter fighter)
        {
            foreach (var part in fighter.Body)
            {
                battleField[part.X, part.Y] = Content.Body;
            }
            battleField[fighter.Tail.X, fighter.Tail.Y] = Content.Tail;
            battleField[fighter.Head.X, fighter.Head.Y] = Content.Head;
        }

        private void CutTail(Fighter fighter)
        {
            battleField[fighter.Tail.X, fighter.Tail.Y] = Content.Empty;
            fighter.CutTail();
            if (fighter.Tail != null)
                battleField[fighter.Tail.X, fighter.Tail.Y] = Content.Tail;
        }

        private void Grow(Fighter fighter, Direction direction)
        {
            battleField[fighter.Head.X, fighter.Head.Y] = Content.Body;
            fighter.Grow(direction);
            battleField[fighter.Head.X, fighter.Head.Y] = Content.Head;
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
