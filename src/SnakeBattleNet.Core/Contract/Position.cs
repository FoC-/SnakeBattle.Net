using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Contract
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Position && Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public override string ToString()
        {
            return "[{0,2},{1,2}]".F(X, Y);
        }
    }
}