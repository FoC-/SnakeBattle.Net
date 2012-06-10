namespace SnakeBattleNet.Core.Common
{
    public class Size
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Size(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}