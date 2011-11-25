using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    class BattleField
    {
        public int SizeX;
        public int SizeY;

        public Matrix CurrentState;

        public BattleField()
        {
            SizeX = 25;
            SizeY = 25;
            CurrentState = new Matrix(SizeX, SizeY);
            SetWalls();
        }

        public BattleField(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            CurrentState = new Matrix(sizeX, sizeY);
            SetWalls();
        }

        private void SetWalls()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (x == 0 || y == 0 || x == SizeX - 1 || y == SizeY - 1)
                    {
                        CurrentState[x, y] = new Row(Content.Wall);
                    }
                    else
                    {
                        CurrentState[x, y] = new Row();
                    }
                }
            }
        }
    }
}
