using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake.Implementation
{
    public class BrainChip : IBrainChip
    {
        private readonly ChipRow[,] chipRows;
        private Move headPosition;

        public BrainChip(Size size)
        {
            Size = size;
            this.chipRows = new ChipRow[Size.X, Size.Y];
            InitilaizeWithHead();
        }

        #region Implement IBrainChip

        public Size Size { get; private set; }

        public Move HeadPosition
        {
            get { return this.headPosition; }
            set
            {
                if (this.headPosition != null)
                    this.chipRows[this.headPosition.X, this.headPosition.Y] = null;

                this.chipRows[value.X, value.Y] = new ChipRow(ChipRowContent.OwnHead);
                this.headPosition = value;
            }
        }

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

        #endregion Implement IBrainChip

        private void InitilaizeWithHead()
        {
            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    chipRows[x, y] = new ChipRow();

            HeadPosition = new Move(Size.X / 2, Size.Y / 2, Direction.North);
            this.chipRows[this.headPosition.X, this.headPosition.Y] = new ChipRow(ChipRowContent.OwnHead);
        }
    }
}
