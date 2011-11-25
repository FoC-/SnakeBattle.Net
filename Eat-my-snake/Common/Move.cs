namespace EatMySnake.Core.Common
{
    public struct Move
    {
        public int x;
        public int y;
        public Direction direction;
        public Move(int x, int y, Direction direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
        }
    }
}
