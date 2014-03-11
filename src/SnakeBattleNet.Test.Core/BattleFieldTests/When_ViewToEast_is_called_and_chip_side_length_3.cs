using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(BattleField))]
    internal class When_ViewToEast_is_called_and_chip_side_length_3 : ViewTestContext
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        Because of = () =>
            result = battleField.ToEast(CreateFighter(), new Position { X = 1, Y = 1 }, 3);

        It should_return_9_elements = () =>
            result.Length.ShouldEqual(9);

        It should_return_4_empty_spaces = () =>
        {
            for (var x = 1; x < 3; x++)
                for (var y = 0; y < 2; y++)
                    result[x, y].Content.ShouldEqual(Content.Empty);
        };

        It should_return_wall_at_left = () =>
        {
            for (var y = 0; y < 3; y++) result[0, y].Content.ShouldEqual(Content.Wall);
        };

        It should_return_wall_at_top = () =>
        {
            for (var x = 0; x < 3; x++) result[x, 2].Content.ShouldEqual(Content.Wall);
        };

        private static BattleField battleField;
        private static ChipCell[,] result;
    }
}