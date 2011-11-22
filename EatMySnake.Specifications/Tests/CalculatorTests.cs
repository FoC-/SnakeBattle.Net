using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EatMySnake.Specifications
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Enter_WhenCalledWithValidNumber_ItShouldReturnTheSameNumberAsWasEnteredAfterPressing_Add()
        {
            //arrange
            Calculator calculator = new Calculator();
            const int numberWasEntred = 10;

            //act
            calculator.Enter(numberWasEntred);

            //assert
            Assert.That(calculator.PressAdd(), Is.EqualTo(numberWasEntred));
        }

        [Test]
        public void Enter_when_called_with_valid_number_it_should_return_the_same_number_as_was_entered_after_pressing__Add()
        {
            //arrange
            Calculator calculator = new Calculator();
            const int numberWasEntred = 10;
            
            //act
            calculator.Enter(numberWasEntred);

            //assert
            Assert.That(calculator.PressAdd(), Is.EqualTo(numberWasEntred));
        }
    }

    // ReSharper restore InconsistentNaming

}
