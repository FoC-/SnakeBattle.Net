namespace SnakeBattleNet.Core.Prototypes
{
    internal interface IMind
    {
        MoveDirection GetMoveDirection(VisibleArea visibleArea);
    }
}