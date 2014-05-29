using System;

namespace SnakeBattleNet.Core.Contract
{
    public class Directed : Position
    {
        public Direction Direction { get; set; }

        public static Directed ToDirection(Position position, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return ToNorthFrom(position);
                case Direction.West:
                    return ToWestFrom(position);
                case Direction.East:
                    return ToEastFrom(position);
                case Direction.South:
                    return ToSouthFrom(position);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public static Directed ToNorthFrom(Position position)
        {
            return new Directed { X = position.X, Y = position.Y + 1, Direction = Direction.North };
        }

        public static Directed ToWestFrom(Position position)
        {
            return new Directed { X = position.X - 1, Y = position.Y, Direction = Direction.West };
        }

        public static Directed ToEastFrom(Position position)
        {
            return new Directed { X = position.X + 1, Y = position.Y, Direction = Direction.East };
        }

        public static Directed ToSouthFrom(Position position)
        {
            return new Directed { X = position.X, Y = position.Y - 1, Direction = Direction.South };
        }

        protected bool Equals(Directed other)
        {
            return Direction == other.Direction
                && X == other.X
                && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Directed)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = X.GetHashCode();
                hash = 31 * hash + Y.GetHashCode();
                return 31 * hash + Direction.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("[{0,2},{1,2}] {2,5}", X, Y, Direction);
        }
    }
}
