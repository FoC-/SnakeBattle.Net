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

        //restuta->foc: this doesn't include direction comparison, why two Move objects with the same X and Y
        //but different directions should be considered as equal?
        public override bool Equals(object obj)
        {
            Move move = (Move)obj;
            return move.X == X && move.Y == Y;
        }

        public override string ToString()
        {
            return String.Format("X={0},Y={1},Direction is {2}", X, Y, direction);
        }
    }
}
