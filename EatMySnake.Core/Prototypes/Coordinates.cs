namespace EatMySnake.Core.Prototypes
{
    public struct Coordinates
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinates(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            //implementation is taken from here: http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
}