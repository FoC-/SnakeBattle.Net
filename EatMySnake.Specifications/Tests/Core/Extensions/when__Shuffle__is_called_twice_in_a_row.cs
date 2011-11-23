using System.Collections.Generic;
using System.Linq;
using EatMySnake.Core.Extensions;
using Machine.Specifications;

namespace EatMySnake.Specifications.Tests.Core.Extensions
{
    [Subject("IEnumerable extensions")]
    public class when__Shuffle__is_called_twice_in_a_row
    {
        Establish context = () =>
                            enumerable = Enumerable.Range(0, 9);

        Because of = () =>
        {
            shuffledEnumerableA = enumerable.Shuffle();
            shuffledEnumerableB = enumerable.Shuffle();
        };

        It should_return_different_sets_of_elements = () =>
            shuffledEnumerableA.ShouldNotEqual(shuffledEnumerableB);

        private static IEnumerable<int> enumerable;
        private static IEnumerable<int> shuffledEnumerableA;
        private static IEnumerable<int> shuffledEnumerableB;
    }
}