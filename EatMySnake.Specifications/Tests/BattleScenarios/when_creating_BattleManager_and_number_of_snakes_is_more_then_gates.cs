using System;
using System.Collections.Generic;
using EatMySnake.Core;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlemanager;
using EatMySnake.Core.Snake;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace EatMySnake.Specifications
{
    [Subject(typeof(BattleManager))]
    class when_creating_BattleManager_and_number_of_snakes_is_more_then_gates
    {
        Establish context = () =>
        {
            int numberOfSnakes = 5;
            int numberOfGateways = 2;

            snakes = Mock.Of<IList<ISnake>>(_ => _.Count == numberOfSnakes);
            battleField = Mock.Of<IBattleField>(_ => _.Gateways.Count == numberOfGateways);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => new BattleManager(battleField, snakes));
        };

        It should_fail = () =>
        {
            exception.ShouldNotBeNull();
        };

        private static Exception exception;
        private static IList<ISnake> snakes;
        private static IBattleField battleField;
    }
}