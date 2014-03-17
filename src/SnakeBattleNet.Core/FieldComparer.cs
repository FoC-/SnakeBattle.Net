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

            var bools = new List<bool>();
            if (chip.Count(c => c.Color == Color.OrBlue) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrBlue).Any(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.OrGreen) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrGreen).Any(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));

            if (chip.Count(c => c.Color == Color.AndGrey) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndGrey).All(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.AndRed) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndRed).All(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.AndBlack) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndBlack).All(c => IsEqual(fighter.BodyParts, field.RelativeCell(direction, fighter.Head, chipHead, c), c)));

            var andType = chipHead.Color == Color.AndBlack || chipHead.Color == Color.AndGrey || chipHead.Color == Color.AndRed;
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