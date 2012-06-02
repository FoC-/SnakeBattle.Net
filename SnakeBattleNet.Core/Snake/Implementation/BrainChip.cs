using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake.Implementation
{
    public class BrainChip : IBrainChip
    {
        private readonly ChipRow[,] _chipRows;
        private Move _headPosition;

        public Size Size { get; private set; }
        public Move HeadPosition
        {
            get { return _headPosition; }
            set
            {
                _chipRows[_headPosition.X, _headPosition.Y] = null;
                _chipRows[value.X, value.Y] = new ChipRow(Content.Head);
                _headPosition = value;
            }
        }

        public BrainChip(Size size)
        {
            Size = size;
            _chipRows = new ChipRow[Size.X, Size.Y];
            InitilaizeWithHead();
        }

        public ChipRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    return _chipRows[x, y];
                }
                return null;
            }
            set
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    _chipRows[x, y] = value;
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
                    rows.Add(_chipRows[x, y]);

            return rows;
        }

        private void InitilaizeWithHead()
        {
            for (int x = 0; x < Size.X; x++)
                for (int y = 0; y < Size.Y; y++)
                    _chipRows[x, y] = null;

            HeadPosition = new Move(Size.X / 2, Size.Y / 2, Direction.North);
            _chipRows[_headPosition.X, _headPosition.Y] = new ChipRow(Content.Head);
        }
    }
}
