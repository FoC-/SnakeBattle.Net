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
                Directed.ToNothFrom(self.Head),
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
                if (self.BodyParts.Count > 3 && self.Tail.X == move.X && self.Tail.Y == move.Y)
                {
                    result.Add(move);
                    continue;
                }

                // Enemy single heads and tails
                foreach (var fighter in fighters)
                {
                    if (fighter.BodyParts.Count == 1 && fighter.Head.X == move.X && fighter.Head.Y == move.Y)
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

        public Direction[] DecidedDirections(Fighter fighter, Direction[] possibleDirections)
        {
            var directions = fighter.Chips
                .Select(cells => possibleDirections.Where(direction => IsEqual(direction, cells.ToList(), fighter)))
                .FirstOrDefault(d => d.Any());
            return directions == null ? possibleDirections : directions.ToArray();
        }

        private bool IsEqual(Direction direction, IList<ChipCell> chip, Fighter fighter)
        {
            var chipHead = chip.First(c => c.Content == Content.Head && c.IsSelf);

            var bools = Color.All.Where(color => chip.Any(c => c.Color == color))
                .Select(color => color.IsAnd
                    ? chip.Where(cell => cell.Color == color)
                        .All(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c))
                    : chip.Where(cell => cell.Color == color)
                        .Any(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)))
                .ToList();

            var andType = Color.All.Any(c => c.IsAnd && c == chipHead.Color);
            return andType
                ? bools.All(b => b)
                : bools.Any(b => b);
        }

        private static bool IsEqual(IEnumerable<Directed> bodyParts, Cell<Content> fieldCell, ChipCell chipCell)
        {
            var fieldIsSelf = bodyParts.Any(m => m.X == fieldCell.X && m.Y == fieldCell.Y);
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Content || chipCell.IsSelf != fieldIsSelf
                : chipCell.Content == fieldCell.Content && chipCell.IsSelf == fieldIsSelf;
        }
    }
}