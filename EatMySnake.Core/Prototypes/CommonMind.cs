using System.Collections.Generic;

namespace EatMySnake.Core.Prototypes
{
    internal interface IMind
    {
        MoveDirection GetNextMoveDirection(VisibleArea visibleArea);
    }

    internal class CommonMind : IMind
    {
        public CommonMind(IEnumerable<MindChip> mindChips)
        {
        }

        public MoveDirection GetNextMoveDirection(VisibleArea visibleArea)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class DummyMind : IMind
    {
        public MoveDirection GetNextMoveDirection(VisibleArea visibleArea)
        {
            return MoveDirection.Forward;
        }
    }
}