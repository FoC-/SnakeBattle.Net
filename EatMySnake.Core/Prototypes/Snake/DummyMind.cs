namespace EatMySnake.Core.Prototypes
{
    /// <summary>
    /// Super dummy mind, can only return random move directionis. (use with care, for really stupid snakes =)
    /// </summary>
    internal class DummyMind : IMind
    {
        public MoveDirection GetMoveDirection(VisibleArea visibleArea)
        {
            return MoveDirection.Forward;
        }
    }
}