namespace SnakeBattleNet.Core.Contract
{
    public class Directed : Position
    {
        public Direction Direction { get; set; }

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
