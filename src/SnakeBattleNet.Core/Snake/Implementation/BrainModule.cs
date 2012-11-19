﻿using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake.Implementation
{
    public class BrainModule : IBrainModule
    {
        private readonly string snakeId;
        private Move ownHead;

        public BrainModule(string id, Size size, string snakeId)
        {
            Id = id;
            Size = size;
            this.snakeId = snakeId;
            ModuleRows = new ModuleRow[Size.X, Size.Y];
            HeadColor = AOColor.AndGrey;
            InitilaizeWithHead();
        }

        #region Implement IBrainModule

        public string Id { get; private set; }
        public Size Size { get; private set; }
        public AOColor HeadColor { get; private set; }

        public ModuleRow[,] ModuleRows { get; private set; }
        public ModuleRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    return ModuleRows[x, y];
                }
                return null;
            }
        }
        public ModuleRow[] ToArray()
        {
            var rows = new List<ModuleRow>();

            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    rows.Add(this.ModuleRows[x, y]);

            return rows.ToArray();
        }

        public void SetWall(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.Wall, exclude, aoColor);
        }

        public void SetEmpty(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.Empty, exclude, aoColor);
        }

        public void SetIndefinied(int x, int y)
        {
            this.ModuleRows[x, y] = new ModuleRow(this.HeadColor);
        }

        public void SetEnemyHead(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.EnemyHead, exclude, aoColor);
        }

        public void SetEnemyBody(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.EnemyBody, exclude, aoColor);
        }

        public void SetEnemyTail(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.EnemyTail, exclude, aoColor);
        }

        public Move GetOwnHead()
        {
            return ownHead;
        }

        public void SetOwnHead(int x, int y, AOColor aoColor, Direction direction)
        {
            if (this.ownHead != null)
                this.ModuleRows[this.ownHead.X, this.ownHead.Y] = null;

            if (this.HeadColor != aoColor)
            {
                this.HeadColor = aoColor;
                PlaceUndefined();
            }

            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnHead, Exclude.No, aoColor, snakeId);
            this.ownHead = new Move(x, y, direction);

        }

        public void SetOwnBody(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnBody, exclude, aoColor, snakeId);
        }

        public void SetOwnTail(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnTail, exclude, aoColor, snakeId);
        }

        #endregion Implement IBrainModule

        private void InitilaizeWithHead()
        {
            PlaceUndefined();

            SetOwnHead(Size.X / 2, Size.Y / 2, this.HeadColor, Direction.North);
        }

        private void PlaceUndefined()
        {
            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    if (this.ModuleRows[x, y] == null || this.ModuleRows[x, y].ModuleRowContent == ModuleRowContent.Undefined)
                        SetIndefinied(x, y);
        }
    }
}