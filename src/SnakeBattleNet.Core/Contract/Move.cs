using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Contract
{
    public class Move
    {
        public Position Position { get; set; }
        public Direction Direction { get; set; }
        public Move(int x, int y, Direction direction)
        {
            Position = new Position { X = x, Y = y };
            Direction = direction;
        }

        public override int GetHashCode()
        {
            return "{0}{1}".F(Position.X, Position.Y).GetHashCode();
        }

        //restuta->foc: this doesn't include direction comparison, why two Move objects with the same X and Y
        //but different directions should be considered as equal?
        public override bool Equals(object obj)
        {
            Move move = (Move)obj;
            return move.Position.X == Position.X && move.Position.Y == Position.Y;
        }

        public override string ToString()
        {
            return "X={0,2},Y={1,2},Direction is {2,5}".F(Position.X, Position.Y, Direction);
        }
    }
}
