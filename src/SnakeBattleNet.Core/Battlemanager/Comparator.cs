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
            const int chipSizeDim = 7;
            var positionOnField = snake.Head.Position;

            //Check if movement is possible
            var possibleMoves = new List<Move>(GetPossibleMoves(battleField, positionOnField));
            if (possibleMoves.Count == 0) return null;

            var moveToNorth = new Move(positionOnField.X, positionOnField.Y - 1, Direction.North);
            var moveToSouth = new Move(positionOnField.X, positionOnField.Y + 1, Direction.South);
            var moveToWest = new Move(positionOnField.X - 1, positionOnField.Y, Direction.West);
            var moveToEast = new Move(positionOnField.X + 1, positionOnField.Y, Direction.East);

            foreach (var chip in snake.Chips)
            {
                var head = chip.SingleOrDefault(c => c.Value.Content == Content.Head && c.Value.IsSelf);
                var positionOnChip = head.Key;

                var northView = battleField.ViewToNorth(positionOnField, positionOnChip, chipSizeDim);
                var westView = battleField.ViewToWest(positionOnField, positionOnChip, chipSizeDim);
                var eastView = battleField.ViewToEast(positionOnField, positionOnChip, chipSizeDim);
                var southView = battleField.ViewToSouth(positionOnField, positionOnChip, chipSizeDim);

#warning Order should be configurable
                if (Compare(northView, chip)) return moveToNorth;
                if (Compare(westView, chip)) return moveToWest;
                if (Compare(eastView, chip)) return moveToEast;
                if (Compare(southView, chip)) return moveToSouth;
            }

            return possibleMoves[new Random().Next(possibleMoves.Count)];
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

        private static IEnumerable<Move> GetPossibleMoves(BattleField battleField, Position position)
        {
            var possibleMoves = new List<Move>();

            var x = position.X;
            var y = position.Y;
            if (battleField[new Position { X = x, Y = y + 1 }] == Content.Empty || battleField[new Position { X = x, Y = y + 1 }] == Content.Tail)
                possibleMoves.Add(new Move(x, y + 1, Direction.North));
            if (battleField[new Position { X = x, Y = y - 1 }] == Content.Empty || battleField[new Position { X = x, Y = y - 1 }] == Content.Tail)
                possibleMoves.Add(new Move(x, y - 1, Direction.South));
            if (battleField[new Position { X = x - 1, Y = y }] == Content.Empty || battleField[new Position { X = x - 1, Y = y }] == Content.Tail)
                possibleMoves.Add(new Move(x - 1, y, Direction.West));
            if (battleField[new Position { X = x + 1, Y = y }] == Content.Empty || battleField[new Position { X = x + 1, Y = y }] == Content.Tail)
                possibleMoves.Add(new Move(x + 1, y, Direction.East));

            return possibleMoves;
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
    }
}