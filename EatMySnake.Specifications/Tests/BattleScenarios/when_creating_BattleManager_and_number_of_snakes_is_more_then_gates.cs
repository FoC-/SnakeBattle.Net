using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Snake;
using Machine.Specifications;
using Moq;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlemanager;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Specifications
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