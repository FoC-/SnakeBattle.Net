using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    internal class ComparerTestContext
    {
        internal static Fighter CreateFighterWithOneChip(List<ChipCell> chip, int x, int y)
        {
            return new Fighter(Guid.NewGuid().ToString(), new[] { chip }, new Directed { X = x, Y = y, Direction = Direction.South });
        }

        internal static List<ChipCell> FullGreyWithOneEnemyTail()
        {
            return new List<ChipCell>
            {
                new ChipCell {X = 5, Y = 6, Content = Content.Tail, Color = Color.Grey()},
                new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = Color.Grey(), IsSelf = true}
            };
        }

        internal static FieldComparer CreateFieldComparer(int ownHeadX, int ownHeadY)
        {
            var battleField = new BattleField();
            battleField[10, 10] = Content.Tail;
            battleField[10, 11] = Content.Body;
            battleField[10, 12] = Content.Head;

            battleField[11, 10] = Content.Tail;
            battleField[11, 11] = Content.Head;

            battleField[12, 10] = Content.Head;
            battleField[ownHeadX, ownHeadY] = Content.Head;
            return new FieldComparer(battleField);
        }
    }
}