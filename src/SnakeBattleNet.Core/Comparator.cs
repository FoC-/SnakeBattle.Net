using System;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Comparator
    {
        public static Move[] PossibleMoves(this View<Content> view, Fighter fighter)
        {
            var moves = new[]
            {
                Move.ToNothFrom(fighter.Head),
                Move.ToWestFrom(fighter.Head),
                Move.ToEastFrom(fighter.Head),
                Move.ToSouthFrom(fighter.Head)
            };

            var possibleMoves = moves.Where(m => IsPossible(view, m)).ToArray();
            if (!possibleMoves.Any()) return possibleMoves;

            foreach (var chip in fighter.Chips)
            {
                var positionOnChip = chip.FirstOrDefault(c => c.Value.Content == Content.Head && c.Value.IsSelf).Key;
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (IsEqual(view.ToNorth(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (IsEqual(view.ToWest(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (IsEqual(view.ToEast(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (IsEqual(view.ToSouth(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        public static View<Tuple<Content, bool>> ToNorth(this View<Content> view, Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head;
            var pairs = new View<Tuple<Content, bool>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    pairs[new Position { X = rx, Y = ry }] = new Tuple<Content, bool>(view[absolutePostion], fighter.BodyParts.Any(m => m == absolutePostion));
                }
            return pairs;
        }

        public static View<Tuple<Content, bool>> ToWest(this View<Content> view, Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head;
            var pairs = new View<Tuple<Content, bool>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    pairs[new Position { X = rx, Y = ry }] = new Tuple<Content, bool>(view[absolutePostion], fighter.BodyParts.Any(m => m == absolutePostion));
                }
            return pairs;
        }

        public static View<Tuple<Content, bool>> ToEast(this View<Content> view, Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head;
            var pairs = new View<Tuple<Content, bool>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    pairs[new Position { X = rx, Y = ry }] = new Tuple<Content, bool>(view[absolutePostion], fighter.BodyParts.Any(m => m == absolutePostion));
                }
            return pairs;
        }

        public static View<Tuple<Content, bool>> ToSouth(this View<Content> view, Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head;
            var pairs = new View<Tuple<Content, bool>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    pairs[new Position { X = rx, Y = ry }] = new Tuple<Content, bool>(view[absolutePostion], fighter.BodyParts.Any(m => m == absolutePostion));
                }
            return pairs;
        }

        private static bool IsEqual(View<Tuple<Content, bool>> field, View<ChipCell> chip)
        {
            var blue = chip.Where(c => c.Value.Color == Color.OrBlue).Any(c => IsEqual(field[c.Key], c.Value));
            var green = chip.Where(c => c.Value.Color == Color.OrGreen).Any(c => IsEqual(field[c.Key], c.Value));

            var grey = chip.Where(c => c.Value.Color == Color.AndGrey).All(c => IsEqual(field[c.Key], c.Value));
            var red = chip.Where(c => c.Value.Color == Color.AndRed).All(c => IsEqual(field[c.Key], c.Value));
            var black = chip.Where(c => c.Value.Color == Color.AndBlack).All(c => IsEqual(field[c.Key], c.Value));

            var color = chip.FirstOrDefault(c => c.Value.IsSelf).Value.Color;
            var andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            return andType
                ? blue && green && grey && red && black
                : blue || green || grey || red || black;
        }

        private static bool IsEqual(Tuple<Content, bool> fieldCell, ChipCell chipCell)
        {
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Item1 || chipCell.IsSelf != fieldCell.Item2
                : chipCell.Content == fieldCell.Item1 && chipCell.IsSelf == fieldCell.Item2;
        }

        private static bool IsPossible(View<Content> battleField, Move move)
        {
            var content = battleField[move];
            return content == Content.Empty || content == Content.Tail;
        }
    }
}