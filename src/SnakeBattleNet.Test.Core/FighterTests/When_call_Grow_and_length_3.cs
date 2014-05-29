using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_Grow_and_length_3
    {
        Establish context = () =>
        {
            fighter = new Fighter("fighter", new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
        };

        Because of = () =>
            fighter.Grow(fighter.Tail.Direction, 2);

        It should_make_fighter_with_head = () =>
            fighter.Head.ShouldEqual(new Directed { X = 10, Y = 12, Direction = Direction.North });

        It should_make_fighter_with_one_body_segment = () =>
            fighter.Body.ShouldContainOnly(new Directed { X = 10, Y = 11, Direction = Direction.North });

        It should_make_fighter_with_tail = () =>
            fighter.Tail.ShouldEqual(new Directed { X = 10, Y = 10, Direction = Direction.North });

        private static Fighter fighter;
    }
}