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
            fighter = CreateDummyFighter(10, 9).AttachChips(FullGreyWithOneEnemyTail());
            var field = CreateFieldForFighters(new[] { fighter });
            comparer = CreateFieldComparer(field);
        };

        Because of = () =>
            result = comparer.DecidedDirections(fighter, new[] { Direction.East, Direction.West, Direction.North, Direction.South, });

        It should_return_move_on_that_row = () =>
            result.ShouldContainOnly(new[] { Direction.North });

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
    }
}