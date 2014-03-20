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

        public Direction[] PossibleDirections(Directed head, IEnumerable<Fighter> fighters)
        {
            var moves = new[]
            {
                Directed.ToNothFrom(head),
                Directed.ToWestFrom(head),
                Directed.ToEastFrom(head),
                Directed.ToSouthFrom(head),
            };
            return moves.Where(m => IsPossible(m, fighters)).Select(_ => _.Direction).ToArray();
        }

        public Direction[] DecidedDirections(Fighter fighter, Direction[] possibleDirections)
        {
            var directions = fighter.Chips
                .Select(c => possibleDirections.Where(direction => IsEqual(direction, c, fighter)))
                .FirstOrDefault(d => d.Any());
            return directions == null ? possibleDirections : directions.ToArray();
        }

        private bool IsPossible(Position position, IEnumerable<Fighter> fighters)
        {
            var x = position.X;
            var y = position.Y;
            return field[x, y] == Content.Empty
                   || field[x, y] == Content.Tail
                   || (field[x, y] == Content.Head && fighters.Any(f => f.BodyParts.Count > 0 && f.Head.X == x && f.Head.Y == y));
        }

        private bool IsEqual(Direction direction, IEnumerable<ChipCell> chip, Fighter fighter)
        {
            var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);
            var colors = Color.GetAll();
            var bools = (from color in colors
                         let cells = chip.Where(c => c.Color.Name == color.Name)
                         where cells.Any()
                         select color.IsAnd
                         ? cells.All(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c))
                         : cells.Any(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c))).ToList();

            var andType = colors.Any(c => c.IsAnd && c.Name == chipHead.Color.Name);
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