using EatMySnake.Core.Prototypes;
using Machine.Specifications;
using NSubstitute;

namespace EatMySnake.Specifications.Core.Prototypes.CoordinatesSpecs
{
    [Subject(typeof(Coordinates), "GetHashCode()")]
    public class when_called_on_two_different_instances_with_different_states
    {
        Establish context = () =>
        {
            coordinates1 = new Coordinates(x: 1, y: Arg.Any<int>());
            coordinates2 = new Coordinates(x: 2, y: Arg.Any<int>());
        };

        Because of = () =>
        {
            hashCode1 = coordinates1.GetHashCode();
            hashCode2 = coordinates2.GetHashCode();
        };

        It should_return_different_hash_codes = () =>
            hashCode1.ShouldNotEqual(hashCode2);

        private static Coordinates coordinates1;
        private static Coordinates coordinates2;
        private static int hashCode1;
        private static int hashCode2;
    }

    [Subject(typeof(Coordinates), "GetHashCode()")]
    public class when_called_on_the_same_instance_with_the_same_state_multiple_times
    {
        Establish context = () =>
        {
            coordinates = new Coordinates(x: 1, y: 2);
        };

        Because of = () =>
        {
            hashCode1 = coordinates.GetHashCode();
            hashCode2 = coordinates.GetHashCode();
        };

        It should_return_the_same_value = () =>
            hashCode1.ShouldEqual(hashCode2);

        private static Coordinates coordinates;
        private static Coordinates coordinates2;
        private static int hashCode1;
        private static int hashCode2;
    }


}