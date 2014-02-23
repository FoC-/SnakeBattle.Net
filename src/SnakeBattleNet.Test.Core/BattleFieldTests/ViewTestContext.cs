using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    internal class ViewTestContext
    {
        internal static Fighter CreateFighter()
        {
            var fighter = new Fighter("id", new List<View<ChipCell>>());
            fighter.Head = new Move(new Position { X = 25, Y = 25 }, Direction.North);
            return fighter;
        }
    }
}