using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    [Subject(typeof(FieldComparer))]
    class When_call_PossibleDirections_case_1 : ComparerTestScenarious
    {
        Establish context = () =>
        {
            fighter = FighterStub.TopRightLengthTwo();
            var field = CreateFieldForFighters(new[] { fighter });
            comparer = CreateFieldComparer(field);
        };

        Because of = () =>
            result = comparer.PossibleDirections(fighter, Enumerable.Empty<Fighter>());

        It should_return_only_west_direction = () =>
            result.ShouldContainOnly(Direction.West);

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
    }
}