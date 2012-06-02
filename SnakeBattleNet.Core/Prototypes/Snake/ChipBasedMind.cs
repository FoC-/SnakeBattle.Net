using System.Collections.Generic;

namespace SnakeBattleNet.Core.Prototypes
{
    internal class ChipBasedMind : IMind
    {
        public ChipBasedMind(IEnumerable<MindChip> mindChips)
        {
        }

        public MoveDirection GetMoveDirection(VisibleArea visibleArea)
        {
            throw new System.NotImplementedException();
        }
    }
}