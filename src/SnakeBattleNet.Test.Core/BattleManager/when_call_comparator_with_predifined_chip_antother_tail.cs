using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(Comparator))]
    class when_call_comparator_with_predifined_chip_antother_tail : stubForRealBattle
    {
        Establish context = () =>
        {
            var module = CreateChipWithAndColoredHead();
            battleField = CreateBattleField();
            fighter = CreateSnakeStub(new[] { module }, battleField);
        };

        Because of = () =>
            result = fighter.PossibleMoves();

        It should_return_move_on_that_row = () =>
            result.First().ShouldEqual(new Move { X = 4, Y = 3, Direction = Direction.North });

        private static Move[] result;
        private static BattleField battleField;
        private static Fighter fighter;
    }
}