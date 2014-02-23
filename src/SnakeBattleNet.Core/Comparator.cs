using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Comparator
    {
        public static Move[] MakeDecision(this BattleField<Content> battleField, Fighter fighter)
        {
            var positionOnField = fighter.Head.Position;
            var moves = new[]
            {
                Move.ToNothFrom(positionOnField),
                Move.ToWestFrom(positionOnField),
                Move.ToEastFrom(positionOnField),
                Move.ToSouthFrom(positionOnField),
            };

            var possibleMoves = moves.Where(m => IsPossible(battleField, m)).ToArray();
            if (!possibleMoves.Any()) return possibleMoves;

            foreach (var chip in fighter.Chips)
            {
                var positionOnChip = chip.FirstOrDefault(c => c.Value.Content == Content.Head && c.Value.IsSelf).Key;
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (Compare(MapViewOnSnake(battleField.ViewToNorth(positionOnField, positionOnChip), fighter), chip))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (Compare(MapViewOnSnake(battleField.ViewToWest(positionOnField, positionOnChip), fighter), chip))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (Compare(MapViewOnSnake(battleField.ViewToEast(positionOnField, positionOnChip), fighter), chip))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (Compare(MapViewOnSnake(battleField.ViewToSouth(positionOnField, positionOnChip), fighter), chip))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        private static bool Compare(IDictionary<Position, Tuple<Content, bool>> view, IEnumerable<KeyValuePair<Position, ChipCell>> chipCells)
        {
            var cells = chipCells.ToList();

            var blue = cells.Where(c => c.Value.Color == Color.OrBlue).Any(c => IsEqual(c.Value, GetFieldCell(view, c.Key)));
            var green = cells.Where(c => c.Value.Color == Color.OrGreen).Any(c => IsEqual(c.Value, GetFieldCell(view, c.Key)));

            var grey = cells.Where(c => c.Value.Color == Color.AndGrey).All(c => IsEqual(c.Value, GetFieldCell(view, c.Key)));
            var red = cells.Where(c => c.Value.Color == Color.AndRed).All(c => IsEqual(c.Value, GetFieldCell(view, c.Key)));
            var black = cells.Where(c => c.Value.Color == Color.AndBlack).All(c => IsEqual(c.Value, GetFieldCell(view, c.Key)));

            var color = cells.FirstOrDefault(c => c.Value.IsSelf).Value.Color;
            var andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            return andType
                ? blue && green && grey && red && black
                : blue || green || grey || red || black;
        }

        private static IDictionary<Position, Tuple<Content, bool>> MapViewOnSnake(BattleField<Tuple<Position, Content>> view, Fighter fighter)
        {
            return view.ToDictionary(k => k.Key, v => new Tuple<Content, bool>(v.Value.Item2, fighter.BodyParts.Any(p => p.Position == v.Value.Item1)));
        }

        private static Tuple<Content, bool> GetFieldCell(IDictionary<Position, Tuple<Content, bool>> view, Position position)
        {
            Tuple<Content, bool> tuple;
            return view.TryGetValue(position, out tuple) ? tuple : new Tuple<Content, bool>(Content.Empty, false);
        }

        private static bool IsEqual(ChipCell chipCell, Tuple<Content, bool> fieldCell)
        {
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Item1 || chipCell.IsSelf != fieldCell.Item2
                : chipCell.Content == fieldCell.Item1 && chipCell.IsSelf == fieldCell.Item2;
        }

        private static bool IsPossible(BattleField<Content> battleField, Move move)
        {
            var content = battleField[move.Position];
            return content == Content.Empty || content == Content.Tail;
        }
    }
}