﻿using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield.Implementation
{
    public class BattleField : IBattleField
    {
        private readonly FieldRow[,] fieldRows;

        public Size Size { get; private set; }
        public string Id { get; private set; }
        public IList<Move> Gateways { get; private set; }
        public FieldRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                    return fieldRows[x, y];
                return null;
            }
            set
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                    fieldRows[x, y] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public BattleField() : this(new Size(27, 27), 1, "field") { }

        public BattleField(Size size, int numberGatewaysOnSide, string id)
        {
            Size = size;
            Id = id;
            fieldRows = new FieldRow[size.X, size.Y];
            CreateGateways(numberGatewaysOnSide);
        }

        /// <summary>
        /// We are looking to north
        /// </summary>
        public FieldRow[] ViewToNorth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(fieldRows[x, y]);

            return rows.ToArray();
        }

        /// <summary>
        /// We are looking to west
        /// </summary>
        public FieldRow[] ViewToWest(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(fieldRows[y, x]);

            return rows.ToArray();
        }

        /// <summary>
        /// We are looking to east
        /// </summary>
        public FieldRow[] ViewToEast(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(fieldRows[y, x]);

            return rows.ToArray();
        }

        /// <summary>
        /// We are looking to south
        /// </summary>
        public FieldRow[] ViewToSouth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim)
        {
            var rows = new List<FieldRow>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(fieldRows[x, y]);

            return rows.ToArray();
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
    }
}