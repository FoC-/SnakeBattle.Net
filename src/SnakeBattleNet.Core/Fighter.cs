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
                var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (IsEqual(this.ToNorth(chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (IsEqual(this.ToWest(chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (IsEqual(this.ToEast(chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (IsEqual(this.ToSouth(chipHead), chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        public ChipCell[,] ToNorth(Position chipHead, int chipSideLength = 7)
        {
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = Head.Y - chipHead.Y; y < Head.Y - chipHead.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = Head.X - chipHead.X; x < Head.X - chipHead.X + chipSideLength; x++, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = Field[x, y],
                        IsSelf = BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public ChipCell[,] ToWest(Position chipHead, int chipSideLength = 7)
        {
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = Head.Y + chipHead.Y; y > Head.Y + chipHead.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = Head.X - chipHead.X; x < Head.X - chipHead.X + chipSideLength; x++, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = Field[x, y],
                        IsSelf = BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public ChipCell[,] ToEast(Position chipHead, int chipSideLength = 7)
        {
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = Head.Y - chipHead.Y; y < Head.Y - chipHead.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = Head.X + chipHead.X; x > Head.X + chipHead.X - chipSideLength; x--, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = Field[x, y],
                        IsSelf = BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public ChipCell[,] ToSouth(Position chipHead, int chipSideLength = 7)
        {
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = Head.Y + chipHead.Y; y > Head.Y + chipHead.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = Head.X + chipHead.X; x > Head.X + chipHead.X - chipSideLength; x--, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = Field[x, y],
                        IsSelf = BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        private bool IsPossible(Position position)
        {
            return Field[position.X, position.Y] == Content.Empty || Field[position.X, position.Y] == Content.Tail;
        }

        private static bool IsEqual(ChipCell[,] view, IEnumerable<ChipCell> chip)
        {
            var blue = chip.Where(c => c.Color == Color.OrBlue).Any(c => IsEqual(view[c.X, c.Y], c));
            var green = chip.Where(c => c.Color == Color.OrGreen).Any(c => IsEqual(view[c.X, c.Y], c));

            var grey = chip.Where(c => c.Color == Color.AndGrey).All(c => IsEqual(view[c.X, c.Y], c));
            var red = chip.Where(c => c.Color == Color.AndRed).All(c => IsEqual(view[c.X, c.Y], c));
            var black = chip.Where(c => c.Color == Color.AndBlack).All(c => IsEqual(view[c.X, c.Y], c));

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