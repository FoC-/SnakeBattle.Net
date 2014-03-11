using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_ViewToEast_is_called_and_chip_side_length_3 : ViewTestContext
    {
        Establish context = () =>
        {
            fighter = CreateFighter();
        };

        Because of = () =>
            result = fighter.ToEast(new Position { X = 1, Y = 1 }, 3);

        It should_return_9_elements = () =>
            result.Length.ShouldEqual(9);

        It should_return_wall_at_left = () =>
        {
            for (var y = 0; y < 3; y++) result[0, y].Content.ShouldEqual(Content.Wall);
        };

        It should_return_wall_at_top = () =>
        {
            for (var x = 0; x < 3; x++) result[x, 2].Content.ShouldEqual(Content.Wall);
        };

        private static ChipCell[,] result;
        private static Fighter fighter;
    }
}