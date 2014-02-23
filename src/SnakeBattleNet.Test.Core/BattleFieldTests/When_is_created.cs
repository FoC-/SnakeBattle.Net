using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(View<Content>))]
    class When_is_created
    {
        Establish context = () =>
        {
            battleField = Build.BattleField();
        };

        It should_exact_number_of_cells = () =>
            battleField.Count().ShouldEqual(Build.BattleFieldSideLength * Build.BattleFieldSideLength);

        It should_exact_number_of_wals = () =>
            battleField.Count(c => c.Value == Content.Wall).ShouldEqual(Build.BattleFieldSideLength * 4 - 4);

        It should_exact_number_of_empty = () =>
            battleField.Count(c => c.Value == Content.Empty).ShouldEqual((Build.BattleFieldSideLength * Build.BattleFieldSideLength) - (Build.BattleFieldSideLength * 4 - 4));

        private static View<Content> battleField;
    }
}
