using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Snake;
using Machine.Specifications;
using Moq;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlemanager;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Specifications.Tests.BattleScenarios
{
    [Subject(typeof(BattleManager))]
    class when_creating_BattleManager_and_number_of_gates_is_more_or_equal_then_snakes
    {
        Establish context = () =>
        {
            int numberOfSnakes = 5;
            int numberOfGateways = 5;

            snakes = Mock.Of<IList<ISnake>>(_ => _.Count == numberOfSnakes);
            battleField = Mock.Of<IBattleField>(_ => _.Gateways.Count == numberOfGateways);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => new BattleManager(battleField, snakes));
        };

        It should_not_fail = () =>
        {
            exception.ShouldBeNull();
        };

        private static Exception exception;
        private static IList<ISnake> snakes;
        private static IBattleField battleField;
    }
}
