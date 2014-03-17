using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    internal class ViewTestContext
    {
        internal static Fighter CreateFighterWithOneChip(BattleField battleField, List<ChipCell> chip, int x, int y)
        {
            return new Fighter(Guid.NewGuid().ToString(), battleField, new[] { chip }, new Directed { X = x, Y = y, Direction = Direction.South });
        }

        internal static Fighter CreateDummyFighter()
        {
            return CreateFighterWithOneChip(new BattleField(), new List<ChipCell>(), 25, 25);
        }

        internal static List<ChipCell> FullGreyWithOneEnemyTail()
        {
            return new List<ChipCell>
            {
                new ChipCell {X = 5, Y = 6, Content = Content.Tail, Color = Color.AndGrey},
                new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = Color.AndGrey, IsSelf = true}
            };
        }

        internal static BattleField CreateBattleField()
        {
            var battleField = new BattleField();
            var fighter1 = new Fighter("fighter1", battleField, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
            fighter1.Grow(fighter1.Head.Direction, 4);
            var fighter2 = new Fighter("fighter2", battleField, new List<IEnumerable<ChipCell>>(), new Directed { X = 11, Y = 10, Direction = Direction.North });
            fighter2.Grow(fighter2.Head.Direction, 4);
            var fighter3 = new Fighter("fighter3", battleField, new List<IEnumerable<ChipCell>>(), new Directed { X = 12, Y = 10, Direction = Direction.North });
            return battleField;
        }
    }
}