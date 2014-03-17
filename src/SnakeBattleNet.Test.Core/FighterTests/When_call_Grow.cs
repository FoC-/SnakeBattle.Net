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
            fighter = new Fighter("fighter", new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
        };

        Because of = () =>
            fighter.Grow(fighter.Head.Direction, 2);

        It should_make_fighter_longer = () =>
            fighter.BodyParts.Count.ShouldEqual(3);

        private static Fighter fighter;
    }
}