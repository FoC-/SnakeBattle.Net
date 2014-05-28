using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    [Subject(typeof(FieldComparer))]
    class When_call_PossibleDirections_case_tails : ComparerTestContext
    {
        Establish context = () =>
        {
            fighter = CreateDummyFighter(11, 12).Run(Direction.South, Direction.East, Direction.North);
            var enemy1 = CreateDummyFighter(13, 12);
            var enemy2 = CreateDummyFighter(12, 13).Run(Direction.North);
            enemies = new[] { enemy1, enemy2 };

            var field = CreateFieldForFighters(new[] { fighter, enemy1, enemy2 });
            comparer = CreateFieldComparer(field);
        };

        Because of = () =>
            result = comparer.PossibleDirections(fighter, enemies);

        It should_return_only_west_direction = () =>
            result.ShouldContainOnly(Direction.West, Direction.North, Direction.East);

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
        private static Fighter[] enemies;
    }
}