using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Contract
{
    public class Move
    {
        public Position Position { get; private set; }
        public Direction Direction { get; private set; }
        public Move(Position position, Direction direction)
        {
            Position = position;
            Direction = direction;
        }

        public static Move ToNothFrom(Position position)
        {
            return new Move(new Position { X = position.X, Y = position.Y + 1 }, Direction.North);
        }

        public static Move ToWestFrom(Position position)
        {
            return new Move(new Position { X = position.X - 1, Y = position.Y }, Direction.West);
        }

        public static Move ToEastFrom(Position position)
        {
            return new Move(new Position { X = position.X + 1, Y = position.Y }, Direction.East);
        }

        public static Move ToSouthFrom(Position position)
        {
            return new Move(new Position { X = position.X, Y = position.Y - 1 }, Direction.South);
        }

        public override string ToString()
        {
            return "{0},Direction is {1,5}".F(Position, Direction);
        }
    }
}
