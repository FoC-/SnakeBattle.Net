using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class FieldComparer
    {
        private readonly BattleField field;

        public FieldComparer(BattleField field)
        {
            this.field = field;
        }

        public Direction[] PossibleDirections(Fighter self, IEnumerable<Fighter> enemies)
        {
            var fighters = enemies as Fighter[] ?? enemies.ToArray();
            var moves = new[]
            {
                Directed.ToNorthFrom(self.Head),
                Directed.ToWestFrom(self.Head),
                Directed.ToEastFrom(self.Head),
                Directed.ToSouthFrom(self.Head),
            };
            var result = new HashSet<Directed>();
            foreach (var move in moves)
            {
                // Empty cells
                if (field[move.X, move.Y] == Content.Empty)
                {
                    result.Add(move);
                    continue;
                }

                // Own tail
                if (self.Body.Count > 1 && self.Tail.X == move.X && self.Tail.Y == move.Y)
                {
                    result.Add(move);
                    continue;
                }

                // Enemy single heads and tails
                foreach (var fighter in fighters)
                {
                    if (fighter.Body.Count == 1 && fighter.Head.X == move.X && fighter.Head.Y == move.Y)
                    {
                        result.Add(move);
                        break;
                    }
                    if (fighter.Tail.X == move.X && fighter.Tail.Y == move.Y)
                    {
                        result.Add(move);
                    }
                }
            }
            return result.Select(_ => _.Direction).ToArray();
        }

        public Tuple<int, Direction[]> DecidedDirections(Fighter fighter, Direction[] possibleDirections)
        {
            var chipNumber = 0;
            foreach (var chip in fighter.Chips)
            {
                var cells = chip.ToList();
                var directions = possibleDirections.Where(d => IsEqual(d, cells, fighter)).ToArray();
                if (directions.Any())
                {
                    return new Tuple<int, Direction[]>(chipNumber, directions);
                }
                chipNumber++;
            }
            return new Tuple<int, Direction[]>(-1, possibleDirections);
        }

        private bool IsEqual(Direction direction, IList<ChipCell> chip, Fighter fighter)
        {
            var chipHead = chip.First(c => c.Content == Content.Head && c.IsSelf);

            var bools = Color.All
                .Where(color => chip.Any(c => c.Color.Name == color.Key))
                .Select(color => color.Value.IsAnd
                    ? chip
                        .Where(cell => cell.Color.Name == color.Key)
                        .All(c => IsEqual(fighter, field.RelativeCell(direction, fighter.Head, chipHead, c), c))
                    : chip
                        .Where(cell => cell.Color.Name == color.Key)
                        .Any(c => IsEqual(fighter, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));

            var andType = Color.All.Any(c => c.Value.IsAnd && c.Key == chipHead.Color.Name);
            return andType
                ? bools.All(b => b)
                : bools.Any(b => b);
        }

        private static bool IsEqual(Fighter fighter, Cell<Content> fieldCell, ChipCell chipCell)
        {
            var fieldIsSelf = FieldIsSelf(fighter, fieldCell);
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Content || chipCell.IsSelf != fieldIsSelf
                : chipCell.Content == fieldCell.Content && chipCell.IsSelf == fieldIsSelf;
        }

        private static bool FieldIsSelf(Fighter fighter, Position fieldCell)
        {
            if (fighter.Head != null && fighter.Head.X == fieldCell.X && fighter.Head.Y == fieldCell.Y) return true;
            if (fighter.Tail != null && fighter.Tail.X == fieldCell.X && fighter.Tail.Y == fieldCell.Y) return true;
            return fighter.Body.Any(b => b.X == fieldCell.X && b.Y == fieldCell.Y);
        }
    }
}