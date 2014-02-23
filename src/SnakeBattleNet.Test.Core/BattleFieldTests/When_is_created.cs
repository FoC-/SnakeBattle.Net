using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(View<Content>))]
    class When_is_created
    {
        Establish context = () =>
        {
            battleField = BattleField.Build();
        };

        It should_exact_number_of_cells = () =>
            battleField.Count().ShouldEqual(BattleField.SideLength * BattleField.SideLength);

        It should_exact_number_of_wals = () =>
            battleField.Count(c => c.Value == Content.Wall).ShouldEqual(BattleField.SideLength * 4 - 4);

        It should_exact_number_of_empty = () =>
            battleField.Count(c => c.Value == Content.Empty).ShouldEqual((BattleField.SideLength * BattleField.SideLength) - (BattleField.SideLength * 4 - 4));

        private static View<Content> battleField;
    }
}
