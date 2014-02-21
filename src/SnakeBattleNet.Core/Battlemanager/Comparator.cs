using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Battlemanager
{
    public static class Comparator
    {
        public static Move MakeDecision(this BattleField battleField, Snake snake)
        {
            var positionOnField = snake.Head.Position;
            var moves = new[]
            {
                Move.ToNothFrom(positionOnField),
                Move.ToWestFrom(positionOnField),
                Move.ToEastFrom(positionOnField),
                Move.ToSouthFrom(positionOnField),
            };

            var possibleMoves = moves.Where(m => IsPossible(battleField, m)).ToArray();
            if (!possibleMoves.Any()) return null;

            foreach (var chip in snake.Chips)
            {
                var positionOnChip = chip.FirstOrDefault(c => c.Value.Content == Content.Head && c.Value.IsSelf).Key;
                foreach (var move in possibleMoves)
                {
                    switch (move.Direction)
                    {
                        case Direction.North:
                            if (Compare(battleField.ViewToNorth(positionOnField, positionOnChip), chip))
                                return move;
                            break;
                        case Direction.West:
                            if (Compare(battleField.ViewToWest(positionOnField, positionOnChip), chip))
                                return move;
                            break;
                        case Direction.East:
                            if (Compare(battleField.ViewToEast(positionOnField, positionOnChip), chip))
                                return move;
                            break;
                        case Direction.South:
                            if (Compare(battleField.ViewToSouth(positionOnField, positionOnChip), chip))
                                return move;
                            break;
                    }
                }
            }
            return possibleMoves[new Random().Next(possibleMoves.Length)];
        }

#warning Self parts are not resolved
        private static bool Compare(IDictionary<Position, Content> fieldCells, IEnumerable<KeyValuePair<Position, ChipCell>> chipCells)
        {
            var cells = chipCells.ToList();

            var blue = cells.Where(c => c.Value.Color == Color.OrBlue).Any(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key)));
            var green = cells.Where(c => c.Value.Color == Color.OrGreen).Any(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key)));

            var grey = cells.Where(c => c.Value.Color == Color.AndGrey).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key)));
            var red = cells.Where(c => c.Value.Color == Color.AndRed).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key)));
            var black = cells.Where(c => c.Value.Color == Color.AndBlack).All(c => IsEqual(c.Value, GetFieldCell(fieldCells, c.Key)));

            var color = cells.FirstOrDefault(c => c.Value.IsSelf).Value.Color;
            var andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            return andType
                ? blue && green && grey && red && black
                : blue || green || grey || red || black;
        }

        private static Content GetFieldCell(IDictionary<Position, Content> fieldCells, Position position)
        {
            Content content;
            return fieldCells.TryGetValue(position, out content) ? content : Content.Empty;
        }

        private static bool IsEqual(ChipCell chipCell, Content fieldCell)
        {
            return chipCell.Exclude
                ? chipCell.Content != fieldCell
                : chipCell.Content == fieldCell;
        }

        private static bool IsPossible(BattleField battleField, Move move)
        {
            var content = battleField[move.Position];
            return content == Content.Empty || content == Content.Tail;
        }
    }
}