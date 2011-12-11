using System;

namespace EatMySnake.Core.Common
{
    public struct Move
    {
        public readonly int X;
        public readonly int Y;
        public readonly Direction direction;
        public Move(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            this.direction = direction;
        }

        public override int GetHashCode()
        {
            return String.Format("{0}{1}",X,Y).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Move move = (Move)obj;
            return move.X == X && move.Y == Y;
        }

        public override string ToString()
        {
            return String.Format("X={0,2},Y={1,2},Direction is {2,5}", X, Y, direction);
        }
    }
}
