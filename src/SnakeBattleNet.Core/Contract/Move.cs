using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Contract
{
    public class Move : Position
    {
        public Direction Direction { get; set; }

        public static Move ToNothFrom(Position position)
        {
            return new Move { X = position.X, Y = position.Y + 1, Direction = Direction.North };
        }

        public static Move ToWestFrom(Position position)
        {
            return new Move { X = position.X - 1, Y = position.Y, Direction = Direction.West };
        }

        public static Move ToEastFrom(Position position)
        {
            return new Move { X = position.X + 1, Y = position.Y, Direction = Direction.East };
        }

        public static Move ToSouthFrom(Position position)
        {
            return new Move { X = position.X, Y = position.Y - 1, Direction = Direction.South };
        }

        public override string ToString()
        {
            return "{0},Direction is {1,5}".F("[{0,2},{1,2}]".F(X, Y), Direction);
        }
    }
}
