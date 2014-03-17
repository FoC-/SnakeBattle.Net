using System.Collections.Generic;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FieldComparerTests
{
    [Subject(typeof(FieldComparer))]
    class When_call_PossibleDirections : ComparerTestContext
    {
        Establish context = () =>
        {
            var battleField = new BattleField();
            fighters = new List<Fighter>();

            var fighter1 = new Fighter("fighter1", new List<IEnumerable<ChipCell>>(), new Directed { X = 2, Y = 2, Direction = Direction.North });
            fighter1.Grow(fighter1.Head.Direction);
            fighters.Add(fighter1);

            var fighter2 = new Fighter("fighter2", new List<IEnumerable<ChipCell>>(), new Directed { X = 3, Y = 1, Direction = Direction.North });
            fighters.Add(fighter2);

            fighter = CreateFighterWithOneChip(FullGreyWithOneEnemyTail(), 2, 1);
            comparer = new FieldComparer(battleField);
        };

        Because of = () =>
            result = comparer.PossibleDirections(fighter.Head, fighters);

        It should_return_north_west_east = () =>
            result.ShouldContainOnly(Direction.North, Direction.West, Direction.East);

        private static Direction[] result;
        private static Fighter fighter;
        private static FieldComparer comparer;
        private static List<Fighter> fighters;
    }
}