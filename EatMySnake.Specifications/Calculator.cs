using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Specifications
{
    /// <summary>
    /// Temporary added class, just to illustrate to FoC how to use SpecFlow
    /// </summary>
    public class Calculator
    {
        private readonly List<int> enteredNumbers = new List<int>(); 

        public void Enter(int number)
        {
            enteredNumbers.Add(number);
        }

        public int PressAdd()
        {
            return enteredNumbers.Sum();
        }
    }
}
