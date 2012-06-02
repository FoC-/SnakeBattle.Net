using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield.Implementation
{
    public class BattleField : IBattleField
    {
        private readonly FieldRow[,] _fieldRows;

        public Size Size { get; private set; }
        public IList<Move> Gateways { get; private set; }
        public FieldRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                    return _fieldRows[x, y];
                return null;
            }
            set
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                    _fieldRows[x, y] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public BattleField() : this(new Size(27, 27), 1) { }

        public BattleField(Size size, int numberGatewaysOnSide)
        {
            Size = size;
            _fieldRows = new FieldRow[size.X, size.Y];
            SetWalls();
            CreateGateways(numberGatewaysOnSide);
        }

        /// <summary>
        /// We are looking to north
        /// </summary>
        public IEnumerable<FieldRow> ViewToNorth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(_fieldRows[x, y]);

            return rows;
        }

        /// <summary>
        /// We are looking to west
        /// </summary>
        public IEnumerable<FieldRow> ViewToWest(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(_fieldRows[y, x]);

            return rows;
        }

        /// <summary>
        /// We are looking to east
        /// </summary>
        public IEnumerable<FieldRow> ViewToEast(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(_fieldRows[y, x]);

            return rows;
        }

        /// <summary>
        /// We are looking to south
        /// </summary>
        public IEnumerable<FieldRow> ViewToSouth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(_fieldRows[x, y]);

            return rows;
        }

        private void CreateGateways(int numberGatewaysOnTheSide)
        {
            Gateways = new List<Move>();
            int x = Size.X / (numberGatewaysOnTheSide + 1);
            int y = Size.Y / (numberGatewaysOnTheSide + 1);

            for (int i = 1; i < numberGatewaysOnTheSide + 1; i++)
            {
                Gateways.Add(new Move(0, i * y, Direction.East));
                Gateways.Add(new Move(Size.X - 1, i * y, Direction.West));
                Gateways.Add(new Move(i * x, 0, Direction.North));
                Gateways.Add(new Move(i * x, Size.Y - 1, Direction.South));
            }
        }

        private void SetWalls()
        {
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                    if (x == 0 || y == 0 || x == Size.X - 1 || y == Size.Y - 1)
                        _fieldRows[x, y] = new FieldRow(FieldRowContent.Wall);
                    else
                        _fieldRows[x, y] = new FieldRow();
        }
    }
}
