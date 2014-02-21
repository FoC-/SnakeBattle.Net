using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(BattleField))]
    internal class When_ViewToSouth_is_called_and_chip_side_length_3
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        Because of = () =>
            result = battleField.ViewToSouth(new Position { X = 25, Y = 25 }, new Position { X = 1, Y = 1 }, 3);

        It should_return_9_elements = () =>
            result.Count().ShouldEqual(9);

        It should_return_4_empty_spaces = () =>
            result.Count(c => c.Value == Content.Empty).ShouldEqual(4);

        It should_return_wall_at_left = () =>
            result.Count(c => c.Key.X == 0 & c.Value == Content.Wall).ShouldEqual(3);

        It should_return_wall_at_bottom = () =>
            result.Count(c => c.Key.Y == 0 && c.Value == Content.Wall).ShouldEqual(3);

        private static BattleField battleField;
        private static IDictionary<Position, Content> result;
    }
}