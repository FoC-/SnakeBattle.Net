using System;
using System.Collections.Generic;

namespace EatMySnake.Core.Prototypes
{
    /// <summary>
    /// Represents a snake.
    /// </summary>
    public class Snake
    {
        public List<MindChip> MindChips { get; set; }
        public Passport Passport { get; set; }
        public Version Version { get; set; }
    }
}