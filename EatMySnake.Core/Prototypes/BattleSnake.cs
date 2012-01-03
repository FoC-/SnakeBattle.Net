namespace EatMySnake.Core.Prototypes
{
    /// <summary>
    /// Represents a snake on a battle field.
    /// </summary>
    public class BattleSnake
    {
        private readonly BattleField battleField;
        private readonly Snake snake;
        private Mind mind;
        private Body body;
        public int SizeOfVisibleArea { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleSnake"/> class.
        /// </summary>
        public BattleSnake(Snake snake)
        {
            this.snake = snake;
            SizeOfVisibleArea = 6;
        }

        public void Move()
        {
            //var moveDirection = mind.GetNextMoveDirection();

            //if (TailOnMyWay(moveDirection))
            //{
            //    battleField.GetTailOwner(tail)
            //}

            //MakeMove(move);

        }
    }
}