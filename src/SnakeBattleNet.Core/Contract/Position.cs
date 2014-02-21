using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Contract
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return "[{0},{1}]".F(X, Y);
        }
    }
}