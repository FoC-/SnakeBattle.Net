using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_call_BiteMove : ViewTestContext
    {
        Establish context = () =>
        {
            var bg = new BattleField();
            bitter = new Fighter("bitter", bg, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 10, Direction = Direction.North });
            bitten = new Fighter("bitten", bg, new List<IEnumerable<ChipCell>>(), new Directed { X = 10, Y = 11, Direction = Direction.North });
            bitten.Grow(3);
        };

        Because of = () =>
            bitter.BiteMove(new[] { bitten }, Direction.North);

        It should_make_biter_longer = () =>
            bitter.BodyParts.Count.ShouldEqual(2);

        It should_make_biten_shoter = () =>
            bitten.BodyParts.Count.ShouldEqual(3);

        private static Fighter bitter;
        private static Fighter bitten;
    }
}