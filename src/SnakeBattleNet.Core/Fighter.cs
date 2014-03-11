using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public BattleField Field { get; private set; }
        public ICollection<IEnumerable<ChipCell>> Chips { get; private set; }
        public LinkedList<Move> BodyParts { get; private set; }

        public Fighter(string id, BattleField field, ICollection<IEnumerable<ChipCell>> chips)
        {
            Field = field;
            Id = id;
            Chips = chips;
            BodyParts = new LinkedList<Move>();
        }

        public int Length { get { return BodyParts.Count; } }
        public Move Head
        {
            get { return Length == 0 ? null : BodyParts.First(); }
            set
            {
                if (Head != null)
                    Field[Head.X, Head.Y] = Content.Body;

                BodyParts.AddFirst(value);
                Field[Head.X, Head.Y] = Content.Head;
            }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            Field[Tail.X, Tail.Y] = Content.Empty;
            BodyParts.RemoveLast();
            Field[Tail.X, Tail.Y] = Content.Tail;
        }

        public void GrowForward()
        {
            switch (Head.Direction)
            {
                case Direction.North:
                    Head = Move.ToNothFrom(Head);
                    break;
                case Direction.West:
                    Head = Move.ToWestFrom(Head);
                    break;
                case Direction.East:
                    Head = Move.ToEastFrom(Head);
                    break;
                case Direction.South:
                    Head = Move.ToSouthFrom(Head);
                    break;
            }
        }

        public void BiteMove(IEnumerable<Fighter> fighters, Move newHeadPosition)
        {
            var fighter = fighters.FirstOrDefault(f => f.Tail.Equals(newHeadPosition));
            if (fighter == null)
            {
                CutTail();
            }
            else
            {
                fighter.CutTail();
            }
            Head = newHeadPosition;
        }

        public Move[] PossibleMoves()
        {
            var moves = new[]
            {
                Move.ToNothFrom(Head),
                Move.ToWestFrom(Head),
                Move.ToEastFrom(Head),
                Move.ToSouthFrom(Head)
            };

            var possibleMoves = moves.Where(IsPossible).ToArray();
            if (!possibleMoves.Any()) return possibleMoves;

            foreach (var chip in Chips)
            {
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (IsEqual(ToNorth, chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (IsEqual(ToWest, chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (IsEqual(ToEast, chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (IsEqual(ToSouth, chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        public ChipCell ToNorth(Position chipHead, Position chipCell)
        {
            var ry = Head.Y - chipHead.Y + chipCell.Y;
            var rx = Head.X - chipHead.X + chipCell.X;
            return new ChipCell { X = rx, Y = ry, Content = Field[rx, ry], IsSelf = BodyParts.Any(m => m.X == rx && m.Y == ry) };
        }

        public ChipCell ToWest(Position chipHead, Position chipCell)
        {
            var ry = Head.Y + chipHead.Y - chipCell.Y;
            var rx = Head.X - chipHead.X + chipCell.X;
            return new ChipCell { X = rx, Y = ry, Content = Field[rx, ry], IsSelf = BodyParts.Any(m => m.X == rx && m.Y == ry) };
        }

        public ChipCell ToEast(Position chipHead, Position chipCell)
        {
            var ry = Head.Y - chipHead.Y + chipCell.Y;
            var rx = Head.X + chipHead.X - chipCell.X;
            return new ChipCell { X = rx, Y = ry, Content = Field[rx, ry], IsSelf = BodyParts.Any(m => m.X == rx && m.Y == ry) };
        }

        public ChipCell ToSouth(Position chipHead, Position chipCell)
        {
            var ry = Head.Y + chipHead.Y - chipCell.Y;
            var rx = Head.X + chipHead.X - chipCell.X;
            return new ChipCell { X = rx, Y = ry, Content = Field[rx, ry], IsSelf = BodyParts.Any(m => m.X == rx && m.Y == ry) };
        }

        private bool IsPossible(Position position)
        {
            return Field[position.X, position.Y] == Content.Empty || Field[position.X, position.Y] == Content.Tail;
        }

        private static bool IsEqual(Func<Position, Position, ChipCell> cellGetter, IEnumerable<ChipCell> chip)
        {
            var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);

            var blue = chip.Where(c => c.Color == Color.OrBlue).Any(c => IsEqual(cellGetter(chipHead, c), c));
            var green = chip.Where(c => c.Color == Color.OrGreen).Any(c => IsEqual(cellGetter(chipHead, c), c));

            var grey = chip.Where(c => c.Color == Color.AndGrey).All(c => IsEqual(cellGetter(chipHead, c), c));
            var red = chip.Where(c => c.Color == Color.AndRed).All(c => IsEqual(cellGetter(chipHead, c), c));
            var black = chip.Where(c => c.Color == Color.AndBlack).All(c => IsEqual(cellGetter(chipHead, c), c));

            var color = chip.FirstOrDefault(c => c.IsSelf && c.Content == Content.Head).Color;
            var andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            return andType
                ? blue && green && grey && red && black
                : blue || green || grey || red || black;
        }

        private static bool IsEqual(ChipCell fieldCell, ChipCell chipCell)
        {
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Content || chipCell.IsSelf != fieldCell.IsSelf
                : chipCell.Content == fieldCell.Content && chipCell.IsSelf == fieldCell.IsSelf;
        }
    }
}