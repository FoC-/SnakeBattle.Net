using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_CutTail
    {
        Establish context = () =>
        {
            fighter = new Fighter("fighter", new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
            fighter.Grow(fighter.Head.Direction, 2);
        };

        Because of = () =>
            fighter.CutTail();

        It should_make_fighter_shorter = () =>
            fighter.BodyParts.Count.ShouldEqual(2);

        private static Fighter fighter;
    }
}