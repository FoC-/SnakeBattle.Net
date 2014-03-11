using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Comparator
    {
        public static Move[] PossibleMoves(this Fighter fighter)
        {
            var moves = new[]
            {
                Move.ToNothFrom(fighter.Head),
                Move.ToWestFrom(fighter.Head),
                Move.ToEastFrom(fighter.Head),
                Move.ToSouthFrom(fighter.Head)
            };

            var possibleMoves = moves.Where(m => IsPossible(fighter.Field, m)).ToArray();
            if (!possibleMoves.Any()) return possibleMoves;

            foreach (var chip in fighter.Chips)
            {
                var chipHead = chip.FirstOrDefault(c => c.Content == Content.Head && c.IsSelf);
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (IsEqual(fighter.Field.ToNorth(fighter, chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (IsEqual(fighter.Field.ToWest(fighter, chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (IsEqual(fighter.Field.ToEast(fighter, chipHead), chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (IsEqual(fighter.Field.ToSouth(fighter, chipHead), chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        public static ChipCell[,] ToNorth(this BattleField field, Fighter fighter, Position chipHead, int chipSideLength = 7)
        {
            var head = fighter.Head;
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = head.Y - chipHead.Y; y < head.Y - chipHead.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = head.X - chipHead.X; x < head.X - chipHead.X + chipSideLength; x++, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = field[x, y],
                        IsSelf = fighter.BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public static ChipCell[,] ToWest(this BattleField field, Fighter fighter, Position chipHead, int chipSideLength = 7)
        {
            var head = fighter.Head;
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = head.Y + chipHead.Y; y > head.Y + chipHead.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = head.X - chipHead.X; x < head.X - chipHead.X + chipSideLength; x++, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = field[x, y],
                        IsSelf = fighter.BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public static ChipCell[,] ToEast(this BattleField field, Fighter fighter, Position chipHead, int chipSideLength = 7)
        {
            var head = fighter.Head;
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = head.Y - chipHead.Y; y < head.Y - chipHead.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = head.X + chipHead.X; x > head.X + chipHead.X - chipSideLength; x--, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = field[x, y],
                        IsSelf = fighter.BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
        }

        public static ChipCell[,] ToSouth(this BattleField field, Fighter fighter, Position chipHead, int chipSideLength = 7)
        {
            var head = fighter.Head;
            var view = new ChipCell[chipSideLength, chipSideLength];
            for (int ry = 0, y = head.Y + chipHead.Y; y > head.Y + chipHead.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = head.X + chipHead.X; x > head.X + chipHead.X - chipSideLength; x--, rx++)
                {
                    view[rx, ry] = new ChipCell
                    {
                        X = rx,
                        Y = ry,
                        Content = field[x, y],
                        IsSelf = fighter.BodyParts.Any(m => m.X == x && m.Y == y)
                    };
                }
            return view;
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

        private static bool IsPossible(BattleField battleField, Position position)
        {
            return battleField[position.X, position.Y] == Content.Empty || battleField[position.X, position.Y] == Content.Tail;
        }
    }
}