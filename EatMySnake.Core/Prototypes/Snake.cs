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

    /// <summary>
    /// Represents a snake on a battle field.
    /// </summary>
    public class BattleSnake
    {
        private readonly BattleField battleField;
        private readonly Snake snake;
        private Mind mind;
        private Body body;
        private VisibleArea visibleArea;


        /// <summary>
        /// Initializes a new instance of the <see cref="BattleSnake"/> class.
        /// </summary>
        public BattleSnake(Snake snake)
        {
            this.snake = snake;
        }
    }
}