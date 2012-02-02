using System.Collections.Generic;

namespace EatMySnake.Core.Prototypes
{
    internal interface IMind
    {
        MoveDirection GetMoveDirection(VisibleArea visibleArea);
    }

    internal class CommonMind : IMind
    {
        public CommonMind(IEnumerable<MindChip> mindChips)
        {
        }

        public MoveDirection GetMoveDirection(VisibleArea visibleArea)
        {
            throw new System.NotImplementedException();
        }
    }

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