using System;
using EatMySnake.Core.Prototypes;
using Machine.Specifications;
using NSubstitute;

namespace EatMySnake.Specifications.Tests.Core
{
    [Subject(typeof (Battle))]
    public class when_created
    {
        Because of = () =>
            battle = new Battle();

        It should_have_default_field_size_that_is_equal_to__25__ = () =>
            battle.FieldSize.ShouldEqual(25);

        It should_have__MaxRounds__set_to_default_value_which_is__500__ = () =>
            battle.MaxRounds.ShouldEqual(500);

        private static Battle battle;
    }
}

