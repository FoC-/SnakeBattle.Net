using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class Comparator
    {
        private readonly IBattleField battleField;
        private readonly ISnake snake;

        public Comparator(IBattleField battleField, ISnake snake)
        {
            this.snake = snake;
            this.battleField = battleField;
        }

        public Move MakeDecision()
        {
            Move headPositionOnBattleField = this.snake.GetHeadPosition();

            var moveToNorth = new Move(headPositionOnBattleField.X, headPositionOnBattleField.Y - 1, Direction.North);
            var moveToSouth = new Move(headPositionOnBattleField.X, headPositionOnBattleField.Y - 1, Direction.North);
            var moveToWest = new Move(headPositionOnBattleField.X, headPositionOnBattleField.Y - 1, Direction.North);
            var moveToEast = new Move(headPositionOnBattleField.X, headPositionOnBattleField.Y - 1, Direction.North);

            foreach (var brainChip in this.snake.BrainModules)
            {
                int chipSizeDim = brainChip.Size.X;
                var headPositionOnBrainChip = brainChip.GetOwnHead();
                var color = brainChip.HeadColor;

                IEnumerable<FieldRow> fieldRows;
                switch (headPositionOnBattleField.direction)
                {
                    case Direction.North:
                        {
                            fieldRows = this.battleField.ViewToNorth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToNorth;
                            fieldRows = this.battleField.ViewToWest(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToWest;
                            fieldRows = this.battleField.ViewToEast(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToEast;
                        }
                        break;
                    case Direction.West:
                        {
                            fieldRows = this.battleField.ViewToWest(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToWest;
                            fieldRows = this.battleField.ViewToSouth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToSouth;
                            fieldRows = this.battleField.ViewToNorth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToNorth;
                        }
                        break;
                    case Direction.East:
                        {
                            fieldRows = this.battleField.ViewToEast(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToEast;
                            fieldRows = this.battleField.ViewToNorth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToNorth;
                            fieldRows = this.battleField.ViewToSouth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToSouth;
                        }
                        break;
                    case Direction.South:
                        {
                            fieldRows = this.battleField.ViewToSouth(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToSouth;
                            fieldRows = this.battleField.ViewToEast(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToEast;
                            fieldRows = this.battleField.ViewToWest(headPositionOnBattleField, headPositionOnBrainChip, chipSizeDim);
                            if (Compare(fieldRows.ToArray(), brainChip.ToEnumeration().ToArray(), color)) return moveToWest;
                        }
                        break;
                }
            }
            return null;
        }

        private bool Compare(FieldRow[] fieldRows, ChipRow[] chipRows, AOColor color)
        {
            bool OrBlue = false;
            int OrBlueCount = 0;
            bool OrGreen = false;
            int OrGreenCount = 0;

            bool AndGrey = true;
            int AndGreyCount = 0;
            bool AndRed = true;
            int AndRedCount = 0;
            bool AndBlack = true;
            int AndBlackCount = 0;

            bool andType = color == AOColor.AndBlack || color == AOColor.AndGrey || color == AOColor.AndRed;

            for (int i = 0; i < fieldRows.Length; i++)
            {
                var equal = chipRows[i].Equals(fieldRows[i]);
                var colorOfRow = chipRows[i].AoColor;
                #region Switch of logic depended on row color

                switch (colorOfRow)
                {
                    case AOColor.OrBlue:
                        {
                            if (andType)
                            {
                                if (equal)
                                    OrBlue = true;
                            }
                            else
                            {
                                if (equal)
                                    return true;
                            }
                            OrBlueCount++;
                        }
                        break;
                    case AOColor.OrGreen:
                        {
                            if (andType)
                            {
                                if (equal)
                                    OrGreen = true;
                            }
                            else
                            {
                                if (equal)
                                    return true;
                            }
                            OrGreenCount++;
                        }
                        break;
                    case AOColor.AndGrey:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    AndGrey = false;
                            }
                            AndGreyCount++;
                        }
                        break;
                    case AOColor.AndRed:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    AndRed = false;
                            }
                            AndRedCount++;
                        }
                        break;
                    case AOColor.AndBlack:
                        {
                            if (andType)
                            {
                                if (!equal)
                                    return false;
                            }
                            else
                            {
                                if (!equal)
                                    AndBlack = false;
                            }
                            AndBlackCount++;
                        }
                        break;
                }

                #endregion Switch of logic depended on row color
            }

            if (andType)
            {
                if (
                    (AndGrey || AndGreyCount == 0) &&
                    (AndRed || AndRedCount == 0) &&
                    (AndBlack || AndBlackCount == 0) &&
                    (OrBlue || OrBlueCount == 0) &&
                    (OrGreen || OrGreenCount == 0)
                   ) return true;
            }
            else
            {
                if (
                    (AndGrey && AndGreyCount > 0) ||
                    (AndRed && AndRedCount > 0) ||
                    (AndBlack && AndBlackCount > 0) ||
                    (OrBlue && OrBlueCount > 0) ||
                    (OrGreen && OrGreenCount > 0)
                   ) return true;
            }

            return false;
        }
    }
}