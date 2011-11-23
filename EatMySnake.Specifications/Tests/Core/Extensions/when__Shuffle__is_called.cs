using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using EatMySnake.Core.Extensions;


namespace EatMySnake.Specifications.Tests.Core.Extensions
{
    [Subject("IEnumerable extensions")]
    public class when__Shuffle__is_called
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
}