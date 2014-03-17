using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    [Subject(typeof(FieldComparer))]
    class When_call_DecidedDirections_with_predifined_chip_antother_tail : ComparerTestContext
    {
        Establish context = () =>
        {
            var battleField = CreateBattleField();
            fighter = CreateFighterWithOneChip(battleField, FullGreyWithOneEnemyTail(), 10, 9);
            comparer = new FieldComparer(battleField);
        };

        Because of = () =>
            result = comparer.DecidedDirections(fighter, new[] { Direction.East, Direction.West, Direction.North, Direction.South, });

        It should_return_move_on_that_row = () =>
            result.Single().ShouldEqual(Direction.North);

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
    }
}