using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BattleField))]
    internal class When_BattleField_ViewToNorth_is_called
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        Because of = () =>
            result = battleField.ViewToNorth(new Position { X = 1, Y = 1 }, new Position { X = 1, Y = 1 }, 3);

        It should_retur_exect_view = () =>
            result.Count(c => c.Value == Content.Wall).ShouldEqual(5);

        private static BattleField battleField;
        private static IDictionary<Position, Content> result;
    }
}