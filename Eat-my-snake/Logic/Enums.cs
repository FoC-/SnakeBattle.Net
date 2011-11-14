namespace EatMySnake.Core.Logic
{
    enum Content
    {
        OwnHead = 1,
        EnemyHead = 2,
        OwnBody = 3,
        EnemyBody = 4,
        OwnTail = 5,
        EnemyTail = 6,
        Wall = 7,
        Empty = 8
    }

    enum Except
    {
        Yes,
        No
    }

    enum AndOrState
    {
        Or_Blue,
        Or_Green,
        And_Grey,
        And_Red,
        And_Black
    }

}