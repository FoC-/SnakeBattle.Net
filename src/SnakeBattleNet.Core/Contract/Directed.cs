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
                    return ToNothFrom(position);
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

        public static Directed ToNothFrom(Position position)
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

        public override string ToString()
        {
            return string.Format("[{0,2},{1,2}] {2,5}", X, Y, Direction);
        }
    }
}
