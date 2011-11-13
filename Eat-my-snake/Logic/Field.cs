using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Logic
{
    class Field
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }

        private Row[,] field;

        public Field(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            this.field = new Row[sizeX, sizeY];
            InitilaizeField();
        }

        private void InitilaizeField()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (x == 0 || y == 0 || x == SizeX - 1 || y == SizeY - 1)
                    {
                        field[x, y] = new Row(Content.Wall);
                    }
                    else
                    {
                        field[x, y] = new Row();
                        int h = field[x, y].GetHashCode();
                    }
                }
            }
        }
    }
}
