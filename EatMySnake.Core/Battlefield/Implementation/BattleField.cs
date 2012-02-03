using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battlefield.Implementation
{
    public class BattleField : IBattleField
    {
        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public IList<Move> Gateways { get; private set; }
        private readonly Row[,] _rows;

        public Row this[int x, int y]
        {
            get
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                    return _rows[x, y];
                return null;
            }
            set
            {
                if (x > -1 && x < SizeX && y > -1 && y < SizeY)
                    _rows[x, y] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public BattleField() : this(27, 27, 1) { }

        public BattleField(int sizeX, int sizeY, int numberGatewaysOnSide)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            _rows = new Row[sizeX, sizeY];
            SetWalls();
            CreateGateways(numberGatewaysOnSide);
        }

        private void CreateGateways(int numberGatewaysOnTheSide)
        {
            Gateways = new List<Move>();
            int x = SizeX / (numberGatewaysOnTheSide + 1);
            int y = SizeY / (numberGatewaysOnTheSide + 1);

            for (int i = 1; i < numberGatewaysOnTheSide + 1; i++)
            {
                Gateways.Add(new Move(0, i * y, Direction.East));
                Gateways.Add(new Move(SizeX - 1, i * y, Direction.West));
                Gateways.Add(new Move(i * x, 0, Direction.North));
                Gateways.Add(new Move(i * x, SizeY - 1, Direction.South));
            }
        }

        private void SetWalls()
        {
            for (int x = 0; x < SizeX; x++)
                for (int y = 0; y < SizeY; y++)
                    if (x == 0 || y == 0 || x == SizeX - 1 || y == SizeY - 1)
                        _rows[x, y] = new Row(Content.Wall);
                    else
                        _rows[x, y] = new Row();
        }
    }
}
