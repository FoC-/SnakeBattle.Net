namespace EatMySnake.Core.Common
{
    public struct Size
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Size(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }
    }
}