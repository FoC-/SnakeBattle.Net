using System;

namespace EatMySnake.Core.Common
{
    public class Matrix
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        protected Row[,] Rows;

        public Matrix(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Rows = new Row[sizeX, sizeY];
        }

        public Row this[int x, int y]
        {
            get
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                {
                    return Rows[x, y];
                }
                return null;
            }
            set
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                {
                    Rows[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
