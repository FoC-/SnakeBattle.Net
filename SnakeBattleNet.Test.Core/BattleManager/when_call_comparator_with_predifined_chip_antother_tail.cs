using System;
using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(Comparator))]
    class when_call_comparator_with_predifined_chip_antother_tail : stubForRealBattle
    {
        private Establish context = () =>
        {
            expectedMove = new Move(4, 3, Direction.North);

            var ownSnake = Guid.NewGuid();

            IBrainChip chip = CreateChipWithAndColoredHead(ownSnake);
            var brainModules = new List<IBrainChip> { chip };

            ISnake snake = CreateSnakeStub(brainModules, ownSnake);
            IBattleField battleField = CreateBattleField(ownSnake);

            comparator = new Comparator(battleField, snake);
        };

        private Because of = () =>
        {
            move = comparator.MakeDecision();
        };

        private It should_return_move_on_that_row = () =>
        {
            move.ShouldEqual(expectedMove);
        };

        private static Move move;
        private static Comparator comparator;
        private static Move expectedMove;
    }
}