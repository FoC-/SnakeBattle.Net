using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    class BattleField : Matrix
    {
        public List<Move> Gateways;

        public BattleField()
            : base(27, 27)
        {
            SetWalls();
            Gateways = new List<Move>
                           {
                               new Move(0, 13, Direction.East),
                               new Move(26, 13, Direction.West),
                               new Move(13, 0, Direction.North),
                               new Move(13, 26, Direction.South)
                           };

        }

        public BattleField(int sizeX, int sizeY)
            : base(sizeX, sizeY)
        {
            SetWalls();
            CreateGateways();
        }

        private void CreateGateways()
        {
            //todo: need to add ability place gateways for different fields
            throw new NotImplementedException();
        }

        private void SetWalls()
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
    }
}
