using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_ViewToWest_is_called_and_chip_side_length_3 : ViewTestContext
    {
        Establish context = () =>
        {
            fighter = CreateFighter();
        };

        Because of = () =>
            result = fighter.ToWest(new Position { X = 1, Y = 1 }, 3);

        It should_return_9_elements = () =>
            result.Length.ShouldEqual(9);

        It should_return_wall_at_right = () =>
        {
            for (var y = 0; y < 3; y++) result[2, y].Content.ShouldEqual(Content.Wall);
        };

        It should_return_wall_at_bottom = () =>
        {
            for (var x = 0; x < 3; x++) result[x, 0].Content.ShouldEqual(Content.Wall);
        };

        private static ChipCell[,] result;
        private static Fighter fighter;
    }
}