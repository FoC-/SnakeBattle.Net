using System;
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

        private Directed Head
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

        private Directed Tail
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

        public void Grow(int length = 1)
        {
            for (var i = 0; i < length; i++)
            {
                Move(Head.Direction, true);
            }
        }

        public void Move(Direction direction, bool isGrow = false)
        {
            switch (direction)
            {
                case Direction.North:
                    Head = Directed.ToNothFrom(Head);
                    break;
                case Direction.West:
                    Head = Directed.ToWestFrom(Head);
                    break;
                case Direction.East:
                    Head = Directed.ToEastFrom(Head);
                    break;
                case Direction.South:
                    Head = Directed.ToSouthFrom(Head);
                    break;
            }
            if (!isGrow)
                MoveTail();
            else if (BodyParts.Count > 1)
                field[Tail.X, Tail.Y] = Content.Tail;
        }

        public void BiteMove(IEnumerable<Fighter> fighters, Direction direction)
        {
            Move(direction, true);
            var fighter = fighters.FirstOrDefault(f => f.Tail.Equals(Head) || (f.Head.Equals(Head) || f.BodyParts.Count == 1));
            if (fighter == null)
            {
                MoveTail();
            }
            else
            {
                fighter.CutTail();
            }
        }

        private void MoveTail()
        {
            field[Tail.X, Tail.Y] = Content.Empty;
            CutTail();
        }

        private void CutTail()
        {
            BodyParts.RemoveLast();
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

            foreach (var chip in chips)
            {
                var list = new List<Direction>();
                foreach (var direction in directions)
                {
                    switch (direction)
                    {
                        case Direction.North:
                            if (IsEqual(ToNorth, chip))
                                list.Add(direction);
                            break;
                        case Direction.West:
                            if (IsEqual(ToWest, chip))
                                list.Add(direction);
                            break;
                        case Direction.East:
                            if (IsEqual(ToEast, chip))
                                list.Add(direction);
                            break;
                        case Direction.South:
                            if (IsEqual(ToSouth, chip))
                                list.Add(direction);
                            break;
                    }
                }
                if (list.Count > 0)
                    return list.ToArray();
            }
            return directions;
        }

        public bool ToNorth(Position chipHead, ChipCell chipCell)
        {
            return IsEqual(new Position { X = Head.X + chipCell.X - chipHead.X, Y = Head.Y + chipCell.Y - chipHead.Y }, chipCell);
        }

        public bool ToWest(Position chipHead, ChipCell chipCell)
        {
            return IsEqual(new Position { X = Head.X - chipCell.Y + chipHead.Y, Y = Head.Y + chipCell.X - chipHead.X }, chipCell);
        }

        public bool ToEast(Position chipHead, ChipCell chipCell)
        {
            return IsEqual(new Position { X = Head.X + chipCell.Y - chipHead.Y, Y = Head.Y - chipCell.X + chipHead.X }, chipCell);
        }

        public bool ToSouth(Position chipHead, ChipCell chipCell)
        {
            return IsEqual(new Position { X = Head.X - chipCell.X + chipHead.X, Y = Head.Y - chipCell.Y + chipHead.Y }, chipCell);
        }

        private bool IsPossible(Position position)
        {
            // todo: single head equal tail
            return field[position.X, position.Y] == Content.Empty || field[position.X, position.Y] == Content.Tail;
        }

        private static bool IsEqual(Func<Position, ChipCell, bool> validator, IEnumerable<ChipCell> chip)
        {
            var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);

            var bools = new List<bool>();
            if (chip.Count(c => c.Color == Color.OrBlue) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrBlue).Any(c => validator(chipHead, c)));
            if (chip.Count(c => c.Color == Color.OrGreen) > 1)
                bools.Add(chip.Where(c => c.Color == Color.OrGreen).Any(c => validator(chipHead, c)));

            if (chip.Count(c => c.Color == Color.AndGrey) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndGrey).All(c => validator(chipHead, c)));
            if (chip.Count(c => c.Color == Color.AndRed) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndRed).All(c => validator(chipHead, c)));
            if (chip.Count(c => c.Color == Color.AndBlack) > 1)
                bools.Add(chip.Where(c => c.Color == Color.AndBlack).All(c => validator(chipHead, c)));

            var andType = chipHead.Color == Color.AndBlack || chipHead.Color == Color.AndGrey || chipHead.Color == Color.AndRed;
            return andType
                ? bools.All(b => b)
                : bools.Any(b => b);
        }

        private bool IsEqual(Position fieldPosition, ChipCell chipCell)
        {
            var fieldContent = field[fieldPosition.X, fieldPosition.Y];
            var fieldIsSelf = BodyParts.Any(m => m.X == fieldPosition.X && m.Y == fieldPosition.Y);

            return chipCell.Exclude
                ? chipCell.Content != fieldContent || chipCell.IsSelf != fieldIsSelf
                : chipCell.Content == fieldContent && chipCell.IsSelf == fieldIsSelf;
        }
    }
}