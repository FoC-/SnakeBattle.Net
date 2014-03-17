using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        private readonly BattleField field;
        private readonly ICollection<IEnumerable<ChipCell>> chips;

        public string Id { get; private set; }
        public LinkedList<Directed> BodyParts { get; private set; }

        public Directed Head
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.First(); }
            set
            {
                if (Head != null)
                    field[Head.X, Head.Y] = Content.Body;

                BodyParts.AddFirst(value);
                field[Head.X, Head.Y] = Content.Head;
            }
        }

        public Directed Tail
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.Last(); }
        }

        public Fighter(string id, BattleField field, ICollection<IEnumerable<ChipCell>> chips, Directed head)
        {
            Id = id;
            BodyParts = new LinkedList<Directed>();
            this.field = field;
            this.chips = chips;
            Head = head;
        }

        public void CutTail()
        {
            // todo: dead
            field[Tail.X, Tail.Y] = Content.Empty;
            BodyParts.RemoveLast();
            if (BodyParts.Count > 1)
                field[Tail.X, Tail.Y] = Content.Tail;
        }

        public void Grow(Direction direction, int length = 1)
        {
            for (var i = 0; i < length; i++)
            {
                Head = Directed.ToDirection(Head, direction);
            }
            field[Tail.X, Tail.Y] = Content.Tail;
        }

        public Direction[] PossibleDirections()
        {
            var moves = new[]
            {
                Directed.ToNothFrom(Head),
                Directed.ToWestFrom(Head),
                Directed.ToEastFrom(Head),
                Directed.ToSouthFrom(Head),
            };

            var directions = moves.Where(IsPossible).Select(_ => _.Direction).ToArray();
            if (!directions.Any()) return directions;

            var possibleDirections = chips
                .Select(c => directions.Where(direction => IsEqual(direction, c)))
                .FirstOrDefault(d => d.Any());

            return possibleDirections == null ? directions : possibleDirections.ToArray();
        }

        private bool IsPossible(Position position)
        {
            // todo: single head equal tail
            return field[position.X, position.Y] == Content.Empty || field[position.X, position.Y] == Content.Tail;
        }

        private bool IsEqual(Direction direction, IEnumerable<ChipCell> chip)
        {
            var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);

            var bools = new List<bool>();
            if (chip.Count(c => c.Color == Color.OrBlue) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrBlue).Any(c => IsEqual(field.RelativeCell(direction, Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.OrGreen) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrGreen).Any(c => IsEqual(field.RelativeCell(direction, Head, chipHead, c), c)));

            if (chip.Count(c => c.Color == Color.AndGrey) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndGrey).All(c => IsEqual(field.RelativeCell(direction, Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.AndRed) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndRed).All(c => IsEqual(field.RelativeCell(direction, Head, chipHead, c), c)));
            if (chip.Count(c => c.Color == Color.AndBlack) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndBlack).All(c => IsEqual(field.RelativeCell(direction, Head, chipHead, c), c)));

            var andType = chipHead.Color == Color.AndBlack || chipHead.Color == Color.AndGrey || chipHead.Color == Color.AndRed;
            return andType
                ? bools.All(b => b)
                : bools.Any(b => b);
        }

        private bool IsEqual(Cell<Content> fieldCell, ChipCell chipCell)
        {
            var fieldIsSelf = BodyParts.Any(m => m.X == fieldCell.X && m.Y == fieldCell.Y);
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Content || chipCell.IsSelf != fieldIsSelf
                : chipCell.Content == fieldCell.Content && chipCell.IsSelf == fieldIsSelf;
        }
    }
}