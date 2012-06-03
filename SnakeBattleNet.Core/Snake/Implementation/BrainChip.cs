using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake.Implementation
{
    public class BrainChip : IBrainChip
    {
        private readonly ChipRow[,] chipRows;
        private readonly Guid snakeId;
        private Move ownHead;
        private AOColor headColor;

        public BrainChip(Size size, Guid snakeId)
        {
            this.snakeId = snakeId;
            Size = size;
            this.chipRows = new ChipRow[Size.X, Size.Y];
            this.headColor = AOColor.AndGrey;
            InitilaizeWithHead();
        }

        #region Implement IBrainChip

        public Size Size { get; private set; }

        public ChipRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    return this.chipRows[x, y];
                }
                return null;
            }
            set
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    this.chipRows[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerable<ChipRow> ToEnumeration()
        {
            var rows = new List<ChipRow>();

            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    rows.Add(this.chipRows[x, y]);

            return rows;
        }

        public void SetWall(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.Wall, exclude, aoColor);
        }

        public void SetEmpty(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.Empty, exclude, aoColor);
        }

        public void SetIndefinied(int x, int y)
        {
            this.chipRows[x, y] = new ChipRow(this.headColor);
        }

        public void SetEnemyHead(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.EnemyHead, exclude, aoColor);
        }

        public void SetEnemyBody(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.EnemyBody, exclude, aoColor);
        }

        public void SetEnemyTail(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.EnemyTail, exclude, aoColor);
        }

        public Move GetOwnHead()
        {
            return ownHead;
        }

        public void SetOwnHead(int x, int y, AOColor aoColor, Direction direction)
        {
            if (this.ownHead != null)
                this.chipRows[this.ownHead.X, this.ownHead.Y] = null;

            if (headColor != aoColor)
            {
                headColor = aoColor;
                PlaceUndefined();
            }

            this.chipRows[x, y] = new ChipRow(ChipRowContent.OwnHead, Exclude.No, aoColor, snakeId);
            this.ownHead = new Move(x, y, direction);

        }

        public void SetOwnBody(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.OwnBody, exclude, aoColor, snakeId);
        }

        public void SetOwnTail(int x, int y, Exclude exclude, AOColor aoColor)
        {
            this.chipRows[x, y] = new ChipRow(ChipRowContent.OwnTail, exclude, aoColor, snakeId);
        }

        #endregion Implement IBrainChip

        private void InitilaizeWithHead()
        {
            PlaceUndefined();

            SetOwnHead(Size.X / 2, Size.Y / 2, this.headColor, Direction.North);
        }

        private void PlaceUndefined()
        {
            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    if (this.chipRows[x, y] == null || this.chipRows[x, y].ChipRowContent == ChipRowContent.Undefined)
                        SetIndefinied(x, y);
        }
    }
}
