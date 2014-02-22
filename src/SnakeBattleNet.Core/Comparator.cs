using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Comparator
    {
        public static Move[] MakeDecision(this BattleField battleField, Fighter fighter)
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
                            if (Compare(battleField.ViewToNorth(positionOnField, positionOnChip), chip, fighter))
                                return new[] { move };
                            break;
                        case Direction.West:
                            if (Compare(battleField.ViewToWest(positionOnField, positionOnChip), chip, fighter))
                                return new[] { move };
                            break;
                        case Direction.East:
                            if (Compare(battleField.ViewToEast(positionOnField, positionOnChip), chip, fighter))
                                return new[] { move };
                            break;
                        case Direction.South:
                            if (Compare(battleField.ViewToSouth(positionOnField, positionOnChip), chip, fighter))
                                return new[] { move };
                            break;
                    }
                }
            }
            return possibleMoves;
        }

        private static bool Compare(IDictionary<Position, Tuple<Position, Content>> fieldCells, IEnumerable<KeyValuePair<Position, ChipCell>> chipCells, Fighter fighter)
        {
            var cells = chipCells.ToList();

            var blue = cells.Where(c => c.Value.Color == Color.OrBlue).Any(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key, fighter)));
            var green = cells.Where(c => c.Value.Color == Color.OrGreen).Any(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key, fighter)));

            var grey = cells.Where(c => c.Value.Color == Color.AndGrey).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key, fighter)));
            var red = cells.Where(c => c.Value.Color == Color.AndRed).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key, fighter)));
            var black = cells.Where(c => c.Value.Color == Color.AndBlack).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key, fighter)));

            var color = cells.FirstOrDefault(c => c.Value.IsSelf).Value.Color;
            var andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            return andType
                ? blue && green && grey && red && black
                : blue || green || grey || red || black;
        }

        private static Tuple<Content, bool> GetFieldCell(IDictionary<Position, Tuple<Position, Content>> fieldCells, Position position, Fighter fighter)
        {
            Tuple<Position, Content> tuple;
            var content = fieldCells.TryGetValue(position, out tuple) ? tuple.Item2 : Content.Empty;
            var isSelf = false;
            if (content == Content.Head || content == Content.Body || content == Content.Tail)
            {
                isSelf = fighter.BodyParts.Any(c => c.Position == tuple.Item1);
            }
            return new Tuple<Content, bool>(content, isSelf);
        }

        private static bool IsEqual(ChipCell chipCell, Tuple<Content, bool> fieldCell)
        {
            return chipCell.Exclude
                ? chipCell.Content != fieldCell.Item1 || chipCell.IsSelf != fieldCell.Item2
                : chipCell.Content == fieldCell.Item1 && chipCell.IsSelf == fieldCell.Item2;
        }

        private static bool IsPossible(BattleField battleField, Move move)
        {
            var content = battleField[move.Position];
            return content == Content.Empty || content == Content.Tail;
        }
    }
}