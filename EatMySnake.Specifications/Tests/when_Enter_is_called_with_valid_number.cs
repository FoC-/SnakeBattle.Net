// ReSharper disable InconsistentNaming
using Machine.Specifications;

namespace EatMySnake.Specifications.Tests
{
    [Subject(typeof(Calculator)), Tags("sample")]
    public class when_correct_number_is_entered
    {
        //arrange
        Establish context = () =>
            calculator = new Calculator();

        //act
        Because of = () => 
            calculator.Enter(NumberWasEntered);

        //assert
        It should_return_the_same_number_as_was_entered_after_pressing__Add__ = () =>
            calculator.PressAdd().ShouldEqual(NumberWasEntered);


        private static Calculator calculator;
        private const int NumberWasEntered = 10;
    }
}
// ReSharper restore InconsistentNaming
