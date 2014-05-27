using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    internal class ComparerTestContext
    {
        internal static Fighter CreateFighter(int x, int y, ICollection<IEnumerable<ChipCell>> chips, params  Direction[] growDirections)
        {
            var fighter = new Fighter(Guid.NewGuid().ToString(), chips, new Directed { X = x, Y = y, Direction = Direction.North });
            foreach (var direction in growDirections)
            {
                fighter.Grow(direction);
            }
            return fighter;
        }

        internal static ICollection<IEnumerable<ChipCell>> FullGreyWithOneEnemyTail()
        {
            return new[]{ new List<ChipCell>
            {
                new ChipCell {X = 5, Y = 6, Content = Content.Tail, Color = new Color.Grey()},
                new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = new Color.Grey(), IsSelf = true}
            }};
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
            if (additionFieldElements != null)
            {
                foreach (var cell in additionFieldElements)
                {
                    battleField[cell.X, cell.Y] = cell.Content;
                }
            }
            return battleField;
        }
    }

    internal class ComparerTestScenarious : ComparerTestContext
    {
        internal static class FighterStub
        {
            internal static Fighter TopRightLengthTwo()
            {
                return CreateFighter(25, 24, GreyEmptyInfront(), Direction.North);
            }

            internal static ICollection<IEnumerable<ChipCell>> GreyEmptyInfront()
            {
                return new[]{ new List<ChipCell>
                {
                    new ChipCell {X = 5, Y = 6, Content = Content.Empty, Color = new Color.Grey()},
                    new ChipCell {X = 5, Y = 5, Content = Content.Head, Color = new Color.Grey(), IsSelf = true}
                }};
            }
        }
    }
}