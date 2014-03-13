using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    [Subject(typeof(Fighter))]
    class When_call_comparator_with_predifined_chip_antother_tail : ViewTestContext
    {
        Establish context = () =>
        {
            var module = CreateChipWithAndColoredHead();
            battleField = CreateBattleField();
            fighter = CreateSnakeStub(new[] { module }, battleField);
        };

        Because of = () =>
            result = fighter.PossibleDirections();

        It should_return_move_on_that_row = () =>
            result.First().ShouldEqual(Direction.North);

        private static Direction[] result;
        private static BattleField battleField;
        private static Fighter fighter;
    }
}