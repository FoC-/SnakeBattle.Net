using System;

namespace EatMySnake.Core.Prototypes
{
    /* var snakeSpecification = new SnakeSpecification()
     * .UseMind<CommonMind>()
     * .UseBody<Body>()
     * .SetSizeOfVisibleAreaTo(6);
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
        private IMind mind;

        public Body Body { get; internal set; }
        public int SizeOfVisibleArea { get; internal set; }

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
            //var visibleArea = battleField.GetVisibleAreaFor(this);
            //var moveDirection = mind.GetMoveDirection(visibleArea);

            //todo: decide what is more logical way of getting tail or tail owner from the battle field in order to bite a tail
            //var moveCoordinates = GetMoveCoordinates(moveDirection);
            //BodyPart tail = battleField.GetTail(moveCoordinates);

            //if (tail != null) //EnemiesTailIsOnMoveDirection()
            //{
            //    var tailOwner = battleField.GetTailOwner(tail);
            //    Bite(tailOwner);
            //    
            //}
            //
            //
            //MoveTo(moveDirection);
            //
        }

        private void MoveTo(dynamic moveDirection)
        {
            /*
             * so we have to options to imlement snake body movement
             * 1) add body part at front and remove from end
             * 2) change each body part coordinate accordingly
             * 
             * so for the first one:
             * 
             * body.Move(moveDirection);
             */
        }

        private void Bite(BattleSnake snakeToBite)
        {
            //fire event: eventBus.Publish(new SnakeBittenEvent(this, snakeToBite))
            snakeToBite.Shorten();
            this.Lengthen();
        }

        private void Lengthen()
        {
            //Body.Add(new BodyPart());
        }

        private void Shorten()
        {
            /*
             * Body.RemovePart();
             * 
             * if (Body.Length == 0)
             *   Die();
             */
        }
    }
}