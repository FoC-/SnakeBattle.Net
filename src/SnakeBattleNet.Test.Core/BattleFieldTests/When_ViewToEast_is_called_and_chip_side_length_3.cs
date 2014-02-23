using System;
using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(View<Content>))]
    internal class When_ViewToEast_is_called_and_chip_side_length_3 : ViewTestContext
    {
        Establish context = () =>
        {
            battleField = BattleField.Build();
        };

        Because of = () =>
            result = battleField.ToEast(CreateFighter(), new Position { X = 1, Y = 1 }, 3);

        It should_return_9_elements = () =>
            result.Count().ShouldEqual(9);

        It should_return_4_empty_spaces = () =>
            result.Count(c => c.Value.Item1 == Content.Empty).ShouldEqual(4);

        It should_return_wall_at_left = () =>
            result.Count(c => c.Key.X == 0 & c.Value.Item1 == Content.Wall).ShouldEqual(3);

        It should_return_wall_at_top = () =>
            result.Count(c => c.Key.Y == 2 && c.Value.Item1 == Content.Wall).ShouldEqual(3);

        private static View<Content> battleField;
        private static View<Tuple<Content, bool>> result;
    }
}