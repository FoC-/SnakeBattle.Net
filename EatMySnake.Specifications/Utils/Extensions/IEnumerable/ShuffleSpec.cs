using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Utils.Extensions;

namespace EatMySnake.Specifications.Utils.Extensions.IEnumerable
{
    [Subject("Shuffle extension")]
    public class when_called
    {
        Establish context = () =>
            enumerable = Enumerable.Range(0, 9);

        Because of = () =>
            suffledEnumerable = enumerable.Shuffle();

        It should_return_elements_in_random_order = () =>
            suffledEnumerable.ShouldNotEqual(enumerable);

        private static IEnumerable<int> enumerable;
        private static IEnumerable<int> suffledEnumerable;
    }

    [Subject("Shuffle extension")]
    public class when_called_twice_in_a_row
    {
        Establish context = () =>
        {
            enumerable = Enumerable.Range(0, 9);
        };

        Because of = () =>
        {
            shuffledEnumerableA = enumerable.Shuffle();
            shuffledEnumerableB = enumerable.Shuffle();
        };

        It should_return_different_sets_of_elements = () =>
        {
            shuffledEnumerableA.ShouldNotEqual(shuffledEnumerableB);
        };


        private static IEnumerable<int> enumerable;
        private static IEnumerable<int> shuffledEnumerableA;
        private static IEnumerable<int> shuffledEnumerableB;
    }


}