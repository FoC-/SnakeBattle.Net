using System;
using Machine.Specifications;
using NSubstitute;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Specifications.Core.Prototypes.BattleFieldSpecs
{
    [Subject(typeof (BattleField))]
    public class when_created_without_parameters
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        It should_have__Size__set_to__25__ = () =>
        {
            battleField.Size.ShouldEqual(25);
        };

        private static BattleField battleField;
    }
}

