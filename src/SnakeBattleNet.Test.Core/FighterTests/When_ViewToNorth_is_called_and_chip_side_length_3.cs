using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    internal class When_ViewToNorth_is_called_and_chip_side_length_3 : ViewTestContext
    {
        Establish context = () =>
        {
            fighter = CreateFighter();
        };

        It should_return_wall_at_right = () =>
        {
            for (var y = 0; y < 3; y++)
                fighter.ToNorth(new Position { X = 1, Y = 1 }, new Position { X = 2, Y = y }).Content.ShouldEqual(Content.Wall);
        };

        It should_return_wall_at_top = () =>
        {
            for (var x = 0; x < 3; x++)
                fighter.ToNorth(new Position { X = 1, Y = 1 }, new Position { X = x, Y = 2 }).Content.ShouldEqual(Content.Wall);
        };

        private static Fighter fighter;
    }
}