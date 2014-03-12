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

        It should_return_own_head = () =>
            fighter.ToEast(new Position { X = 1, Y = 1 }, new ChipCell { X = 1, Y = 1, Content = Content.Head, IsSelf = true }).ShouldBeTrue();

        It should_return_wall_at_left = () =>
        {
            for (var y = 0; y < 3; y++)
                fighter.ToEast(new Position { X = 1, Y = 1 }, new ChipCell { X = 0, Y = y, Content = Content.Wall }).ShouldBeTrue();
        };

        It should_return_wall_at_top = () =>
        {
            for (var x = 0; x < 3; x++)
                fighter.ToEast(new Position { X = 1, Y = 1 }, new ChipCell { X = x, Y = 2, Content = Content.Wall }).ShouldBeTrue();
        };

        private static Fighter fighter;
    }
}