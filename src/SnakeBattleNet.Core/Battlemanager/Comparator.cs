using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class Comparator
    {
        private readonly BattleField _battleField;
        private readonly Snake _snake;

        public Comparator(BattleField battleField, Snake snake)
        {
            _snake = snake;
            _battleField = battleField;
        }

        public Move MakeDecision()
        {
            const int chipSizeDim = 7;
            var positionOnField = _snake.Head.Position;

            var moveToNorth = new Move(positionOnField.X, positionOnField.Y - 1, Direction.North);
            var moveToSouth = new Move(positionOnField.X, positionOnField.Y + 1, Direction.South);
            var moveToWest = new Move(positionOnField.X - 1, positionOnField.Y, Direction.West);
            var moveToEast = new Move(positionOnField.X + 1, positionOnField.Y, Direction.East);

            foreach (var cells in _snake.Chips)
            {
                var chip = cells.ToList();
                var head = chip.SingleOrDefault(c => c.Content == Content.Head && c.IsSelf);
                if (head == null) return null;
                var positionOnChip = head.Position;
                var color = head.Color;

                IEnumerable<FieldCell> fieldRows;
                switch (_snake.Head.Direction)
                {
                    case Direction.North:
                        {
                            fieldRows = _battleField.ViewToNorth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToNorth;
                            fieldRows = _battleField.ViewToWest(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToWest;
                            fieldRows = _battleField.ViewToEast(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToEast;
                        }
                        break;
                    case Direction.West:
                        {
                            fieldRows = _battleField.ViewToWest(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToWest;
                            fieldRows = _battleField.ViewToSouth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToSouth;
                            fieldRows = _battleField.ViewToNorth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToNorth;
                        }
                        break;
                    case Direction.East:
                        {
                            fieldRows = _battleField.ViewToEast(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToEast;
                            fieldRows = _battleField.ViewToNorth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToNorth;
                            fieldRows = _battleField.ViewToSouth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToSouth;
                        }
                        break;
                    case Direction.South:
                        {
                            fieldRows = _battleField.ViewToSouth(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToSouth;
                            fieldRows = _battleField.ViewToEast(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToEast;
                            fieldRows = _battleField.ViewToWest(positionOnField, positionOnChip, chipSizeDim);
                            if (Compare(fieldRows, chip, color)) return moveToWest;
                        }
                        break;
                }
            }
            return null;
        }

        private bool Compare(IEnumerable<FieldCell> fieldCells, IEnumerable<ChipCell> chipCells, Color color)
        {
            var orBlue = false;
            var orBlueCount = 0;
            var orGreen = false;
            var orGreenCount = 0;

            var andGrey = true;
            var andGreyCount = 0;
            var andRed = true;
            var andRedCount = 0;
            var andBlack = true;
            var andBlackCount = 0;

            bool andType = color == Color.AndBlack || color == Color.AndGrey || color == Color.AndRed;
            for (int i = 0; i < fieldCells.Count(); i++)
            {
                var equal = chipCells[i].Equals(fieldCells[i]);
                var colorOfRow = chipCells[i].Color;
                #region Switch of logic depended on row color

                switch (colorOfRow)
                {
                    case Color.OrBlue:
                        {
                            if (andType)
                            {
                                if (equal)
                                    orBlue = true;
                            }
                            else
                            {
                                if (equal)
                                    return true;
                            }
                            orBlueCount++;
                        }
                        break;
                    case Color.OrGreen:
                        {
                            if (andType)
                            {
                                if (equal)
                                    orGreen = true;
                            }
                            else
                            {
                                if (equal)
                                    return true;
                            }
                            orGreenCount++;
                        }
                        break;
                    case Color.AndGrey:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    andGrey = false;
                            }
                            andGreyCount++;
                        }
                        break;
                    case Color.AndRed:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    andRed = false;
                            }
                            andRedCount++;
                        }
                        break;
                    case Color.AndBlack:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    andBlack = false;
                            }
                            andBlackCount++;
                        }
                        break;
                }

                #endregion Switch of logic depended on row color
            }

            if (andType)
            {
                if (
                    (andGrey || andGreyCount == 0) &&
                    (andRed || andRedCount == 0) &&
                    (andBlack || andBlackCount == 0) &&
                    (orBlue || orBlueCount == 0) &&
                    (orGreen || orGreenCount == 0)
                   ) return true;
            }
            else
            {
                if (
                    (andGrey && andGreyCount > 0) ||
                    (andRed && andRedCount > 0) ||
                    (andBlack && andBlackCount > 0) ||
                    (orBlue && orBlueCount > 0) ||
                    (orGreen && orGreenCount > 0)
                   ) return true;
            }
            return false;
        }
    }
}