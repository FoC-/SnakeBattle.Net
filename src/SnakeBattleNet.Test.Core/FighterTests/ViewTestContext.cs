using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    internal class ViewTestContext
    {
        internal static Fighter CreateFighter()
        {
            return new Fighter("id", new BattleField(), new List<IEnumerable<ChipCell>>(), new Directed { X = 25, Y = 25, Direction = Direction.North });
        }

        internal static Fighter CreateSnakeStub(BattleField battleField)
        {
            var chip = new List<ChipCell>
            {
                new ChipCell {X = 5, Y = 6, Content = Content.Tail, Color = Color.AndGrey},
                new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = Color.AndGrey, IsSelf = true}
            };
            return new Fighter(Guid.NewGuid().ToString(), battleField, new[] { chip }, new Directed { X = 10, Y = 9, Direction = Direction.South });
        }

        internal static BattleField CreateBattleField()
        {
            var battleField = new BattleField();
            var fighter1 = new Fighter("fighter1", battleField, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
            fighter1.Grow(4);
            var fighter2 = new Fighter("fighter2", battleField, new List<IEnumerable<ChipCell>>(), new Directed { X = 11, Y = 10, Direction = Direction.North });
            fighter2.Grow(4);
            return battleField;
        }
    }
}