using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace EatMySnake.Specifications
{
    [Binding]
    public class AddtionFeatureDefinition
    {
        private readonly Calculator calculator = new Calculator();
        private int resultOnTheScreen;

        [Given(@"I have entered 50 into the calculator ")]
        public void GivenIHaveEntered50IntoTheCalculator()
        {
            calculator.Enter(50);
        }

        [Given(@"I have entered 70 into the calculator")]
        public void GivenIHaveEntered70IntoTheCalculator()
        {
            calculator.Enter(70);
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            resultOnTheScreen = calculator.PressAdd();
        }

        [Then(@"the result should be 120 on the screen")]
        public void ThenTheResultShouldBe120OnTheScreen()
        {
            Assert.That(resultOnTheScreen, Is.EqualTo(120));
        }

    }
}
