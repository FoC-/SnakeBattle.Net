using Machine.Specifications;

namespace EatMySnake.Specifications.Tests
{
    [Subject(typeof(Calculator))]
    public class when_Enter_is_called_with_valid_number
    {
        Establish context = () =>
            calculator = new Calculator();

        Because of = () => 
            calculator.Enter(numberWasEntered);

        It should_return_the_same_number_as_was_entered_after_pressing__Add__ = () =>
            calculator.PressAdd().ShouldEqual(numberWasEntered);

        private static Calculator calculator;
        private static int numberWasEntered = 10;
    }
}