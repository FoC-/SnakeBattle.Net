using System.Collections.Generic;

namespace SnakeBattleNet.Core.Prototypes
{
    internal interface IBattleField
    {
        void AddSnakes(IList<BattleSnake> snakes);
    }
}