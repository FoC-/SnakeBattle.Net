using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    [Subject(typeof(FieldComparer))]
    class When_call_PossibleDirections_case_blocked : ComparerTestContext
    {
        Establish context = () =>
        {
            fighter = CreateDummyFighter(25, 22).Run(Direction.North);
            var enemy1 = CreateDummyFighter(24, 22).Run(Direction.North);
            var enemy2 = CreateDummyFighter(25, 25).Run(Direction.South, Direction.West);
            enemies = new[] { enemy1, enemy2 };

            var field = CreateFieldForFighters(new[] { fighter, enemy1, enemy2 });
            comparer = CreateFieldComparer(field);
        };

        Because of = () =>
            result = comparer.PossibleDirections(fighter, enemies);

        It should_not_return_any_directions = () =>
            result.ShouldBeEmpty();

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
        private static Fighter[] enemies;
    }
}