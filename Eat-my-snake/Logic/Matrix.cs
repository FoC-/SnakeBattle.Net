using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Logic
{
    public class Matrix
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public Row[,] Rows { get; private set; }

        public Matrix(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            this.Rows = new Row[sizeX, sizeY];
        }

        public void InitilaizeFieldWithWalls()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (x == 0 || y == 0 || x == SizeX - 1 || y == SizeY - 1)
                    {
                        Rows[x, y] = new Row(Content.Wall);
                    }
                    else
                    {
                        Rows[x, y] = new Row();
                    }
                }
            }
        }

        public void InitilaizeFieldWithHead()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Rows[x, y] = new Row();
                }
            }
            int xh = SizeX / 2;
            int yh = SizeY / 2;
            Rows[xh, yh] = new Row(Content.OwnHead);
        }
    }
}
