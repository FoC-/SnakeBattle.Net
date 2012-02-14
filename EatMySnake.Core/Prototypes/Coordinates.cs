namespace EatMySnake.Core.Prototypes
{
    public struct Coordinates
    {
        public Coordinates(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}