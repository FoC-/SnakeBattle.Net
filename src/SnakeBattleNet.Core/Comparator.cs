using System;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Comparator
    {
        public static Move[] MakeDecision(this View<Content> view, Fighter fighter)
        {
            var moves = new[]
            {
                Move.ToNothFrom(fighter.Head.Position),
                Move.ToWestFrom(fighter.Head.Position),
                Move.ToEastFrom(fighter.Head.Position),
                Move.ToSouthFrom(fighter.Head.Position)
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
                            if (Compare(view.ToNorth(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (Compare(view.ToWest(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (Compare(view.ToEast(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (Compare(view.ToSouth(fighter, positionOnChip), chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        private static bool Compare(View<Tuple<Content, bool>> fieldView, View<ChipCell> chipView)
        {
            var cells = chipView.ToList();

            var blue = cells.Where(c => c.Value.Color == Color.OrBlue).Any(c => IsEqual(fieldView[c.Key], c.Value));
            var green = cells.Where(c => c.Value.Color == Color.OrGreen).Any(c => IsEqual(fieldView[c.Key], c.Value));

            var grey = cells.Where(c => c.Value.Color == Color.AndGrey).All(c => IsEqual(fieldView[c.Key], c.Value));
            var red = cells.Where(c => c.Value.Color == Color.AndRed).All(c => IsEqual(fieldView[c.Key], c.Value));
            var black = cells.Where(c => c.Value.Color == Color.AndBlack).All(c => IsEqual(fieldView[c.Key], c.Value));

            var color = cells.FirstOrDefault(c => c.Value.IsSelf).Value.Color;
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
            var content = battleField[move.Position];
            return content == Content.Empty || content == Content.Tail;
        }
    }
}