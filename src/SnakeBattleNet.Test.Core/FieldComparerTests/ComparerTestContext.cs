using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    internal class ComparerTestContext
    {
        internal static Fighter CreateDummyFighter(int x, int y)
        {
            return new Fighter(Guid.NewGuid().ToString(), new List<IEnumerable<ChipCell>>(), new Directed { X = x, Y = y, Direction = Direction.North });
        }

        internal static IEnumerable<ChipCell> FullGreyWithOneEnemyTail()
        {
            return new List<ChipCell>
            {
                new ChipCell {X = 5, Y = 6, Content = Content.Tail, Color = Color.Grey()},
                new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = Color.Grey(), IsSelf = true}
            };
        }

        internal static FieldComparer CreateFieldComparer(BattleField battleField)
        {
            return new FieldComparer(battleField);
        }

        internal static BattleField CreateFieldForFighters(IEnumerable<Fighter> fighters, params Cell<Content>[] additionFieldElements)
        {
            var battleField = new BattleField();
            foreach (var fighter in fighters)
            {
                foreach (var part in fighter.BodyParts)
                {
                    battleField[part.X, part.Y] = Content.Body;
                }
                battleField[fighter.Tail.X, fighter.Tail.Y] = Content.Tail;
                battleField[fighter.Head.X, fighter.Head.Y] = Content.Head;
            }
            foreach (var cell in additionFieldElements)
            {
                battleField[cell.X, cell.Y] = cell.Content;
            }
            return battleField;
        }
    }

    internal static class Helpers
    {
        internal static Fighter Run(this Fighter fighter, Direction direction, int times = 1)
        {
            for (var i = 0; i < times; i++)
                fighter.Grow(Direction.North);
            return fighter;
        }

        internal static Fighter Run(this Fighter fighter, params Direction[] directions)
        {
            foreach (var direction in directions)
                fighter.Grow(direction);
            return fighter;
        }

        internal static Fighter AttachChips(this Fighter fighter, params IEnumerable<ChipCell>[] chips)
        {
            foreach (var chip in chips)
                fighter.Chips.Add(chip);
            return fighter;
        }
    }
}