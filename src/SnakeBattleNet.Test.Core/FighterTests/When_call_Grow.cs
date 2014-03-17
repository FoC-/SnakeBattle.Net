using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_Grow
    {
        Establish context = () =>
        {
            field = new BattleField();
            fighter = new Fighter("fighter", field, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
        };

        Because of = () =>
            fighter.Grow(fighter.Head.Direction, 2);

        It should_make_fighter_longer = () =>
            fighter.BodyParts.Count.ShouldEqual(3);

        It should_put_head_on_field = () =>
           field[10, 12].ShouldEqual(Content.Head);

        It should_put_body_on_field = () =>
           field[10, 11].ShouldEqual(Content.Body);

        It should_put_tail_on_field = () =>
           field[10, 10].ShouldEqual(Content.Tail);

        private static Fighter fighter;
        private static BattleField field;
    }
}