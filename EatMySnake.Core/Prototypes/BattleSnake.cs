namespace EatMySnake.Core.Prototypes
{
    /* var snakeSpecification = new SnakeSpecification()
     * .UsingMind<Mind>()
     * .UsingBody<Body>()
     * .SizeOfVisibleArea(6);
     * 
     * var battleSnake = battleSnakeBuilder.BuildFrom(snake).Using(snakeSpecification);
     * */


    /// <summary>
    /// Represents a snake on a battle field.
    /// </summary>
    public class BattleSnake
    {
        private readonly BattleField battleField;
        private readonly Snake snake;
        private Mind mind;

        public Body Body { get; private set; }
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

            //var moveCoordinates = GetMoveCoordinates(moveDirection);
            //BodyPart tail = battleField.GetTail(moveCoordinates);
            //

            //if (tail != null)
            //{
            //    var tailOwner = battleField.GetTailOwner(tail);
            //    Bite(tailOwner);
            //    
            //}
            //else
            //{
            //   MakeMove(moveDirection);
            //}

            //MakeMove(move);
        }


    }
}