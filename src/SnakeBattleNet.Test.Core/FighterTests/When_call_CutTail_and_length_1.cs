using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_CutTail_and_length_1
    {
        Establish context = () =>
        {
            fighter = new Fighter("fighter", new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
        };

        Because of = () =>
            fighter.CutTail();

        It should_make_fighter_without_head = () =>
            fighter.Head.ShouldBeNull();

        It should_make_fighter_without_body = () =>
            fighter.Body.ShouldBeEmpty();

        It should_make_fighter_without_tail = () =>
            fighter.Tail.ShouldBeNull();

        private static Fighter fighter;
    }
}