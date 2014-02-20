using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BattleField))]
    class When_BattleField_is_created
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        It should_exact_number_of_cells = () =>
            battleField.Cells.Count.ShouldEqual(battleField.SideLength * battleField.SideLength);

        It should_exact_number_of_wals = () =>
            battleField.Cells.Count(c => c.Value == Content.Wall).ShouldEqual(battleField.SideLength * 4 - 4);

        It should_exact_number_of_empty = () =>
            battleField.Cells.Count(c => c.Value == Content.Empty).ShouldEqual((battleField.SideLength * battleField.SideLength) - (battleField.SideLength * 4 - 4));

        It should_exact_number_of_gateways = () =>
            battleField.Gateways.Count.ShouldEqual(4);

        private static BattleField battleField;
    }
}
