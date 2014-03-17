using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(BattleField))]
    internal class When_RelativeCell_is_called_and_direction_is_north
    {
        Establish context = () =>
        {
            field = new BattleField();
        };

        It should_return_wall_at_right = () =>
        {
            for (var y = 0; y < 3; y++)
                field.RelativeCell(Direction.North,
                   new Position { X = 25, Y = 25 },
                   new Position { X = 1, Y = 1 },
                   new Position { X = 2, Y = y })
                   .Content.ShouldEqual(Content.Wall);
        };

        It should_return_wall_at_top = () =>
        {
            for (var x = 0; x < 3; x++)
                field.RelativeCell(Direction.North,
                   new Position { X = 25, Y = 25 },
                   new Position { X = 1, Y = 1 },
                   new Position { X = x, Y = 2 })
                   .Content.ShouldEqual(Content.Wall);
        };

        private static BattleField field;
    }
}