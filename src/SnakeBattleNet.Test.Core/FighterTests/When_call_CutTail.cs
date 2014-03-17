using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_CutTail : ViewTestContext
    {
        Establish context = () =>
        {
            field = new BattleField();
            fighter = new Fighter("fighter", field, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
            fighter.Grow(fighter.Head.Direction, 2);
        };

        Because of = () =>
            fighter.CutTail();

        It should_make_fighter_shorter = () =>
            fighter.BodyParts.Count.ShouldEqual(2);

        It should_put_tail_on_field = () =>
            field[fighter.Tail.X, fighter.Tail.Y].ShouldEqual(Content.Tail);

        It should_not_contain_tail_on_previous_cell = () =>
            field[10, 10].ShouldEqual(Content.Empty);

        private static Fighter fighter;
        private static BattleField field;
    }
}