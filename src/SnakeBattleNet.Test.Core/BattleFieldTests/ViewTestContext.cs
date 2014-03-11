using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    internal class ViewTestContext
    {
        internal static Fighter CreateFighter()
        {
            var fighter = new Fighter("id", new BattleField(), new List<IEnumerable<ChipCell>>());
            fighter.Head = new Move { X = 25, Y = 25, Direction = Direction.North };
            return fighter;
        }
    }
}