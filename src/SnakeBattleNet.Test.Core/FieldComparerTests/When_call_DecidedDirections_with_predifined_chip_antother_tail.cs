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
            fighter = CreateDummyFighter(10, 8).Run(Direction.North).AttachChips(FullGreyWithOneEnemyTail());
            var enemy1 = CreateDummyFighter(10, 10);
            var enemy2 = CreateDummyFighter(11, 9).Run(Direction.North);
            enemies = new[] { enemy1, enemy2 };

            var field = CreateFieldForFighters(new[] { fighter, enemy1, enemy2 });
            comparer = CreateFieldComparer(field);
        };

        Because of = () =>
            result = comparer.DecidedDirections(fighter, comparer.PossibleDirections(fighter, enemies));

        It should_return_move_on_that_row = () =>
            result.ShouldContainOnly(Direction.North, Direction.East);

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
        private static Fighter[] enemies;
    }
}