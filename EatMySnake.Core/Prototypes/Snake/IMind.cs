namespace EatMySnake.Core.Prototypes
{
    internal interface IMind
    {
        MoveDirection GetMoveDirection(VisibleArea visibleArea);
    }
}