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
            battleField = CreateBattleField();
            fighter = CreateSnakeStub(battleField);
        };

        Because of = () =>
            result = fighter.PossibleDirections();

        It should_return_move_on_that_row = () =>
            result.Single().ShouldEqual(Direction.North);

        private static Direction[] result;
        private static BattleField battleField;
        private static Fighter fighter;
    }
}