using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Prototypes
{
    class SnakePrototype : ISnake
    {
        private VisibleArea visibleArea;
        private List<LogicalChip> logicalChips;

        public int BodyLength
        {
            get { throw new NotImplementedException(); }
        }

        private void Bite(SnakePrototype snakeToBite)
        {
            //fire event: eventBus.Publish(new SnakeBitedEvent(this, snakeToBite))
            snakeToBite.Shorten();
            this.Grow();
        }

        private void Grow()
        {
            throw new NotImplementedException();
        }

        private void Shorten()
        {
            throw new NotImplementedException();
        }

        public void Move()
        {
            MoveDirection moveDirection = GetNextMoveDirection();

           /* if (EnemiesTailIsOnMoveDirection())
            * {
            *   var tailOwner = GetTailOwner(...);
            *   Bite(tailOwner);
            * }
            */
        }

        private MoveDirection GetNextMoveDirection()
        {
            /* List<MoveDirection> movingDirections;
             * movingDirections = movingDirections.Shuffle();
             *  get move direction using visibleArea and logicalChips
             */

            throw new NotImplementedException();
        }
    }

    internal enum MoveDirection
    {
        Left,
        Right,
        Forward,
        None
    }

    internal class LogicalChip
    {
    }

    internal class VisibleArea
    {
    }

    /*
     * snake spawns event like: moved to coordina
     */

    interface ISnake
    {
        int BodyLength { get; }
    }


    class BattlePrototype
    {
        private IEnumerable<SnakePrototype> snakes;

        public void NextTurn()
        {
            //foreach (var snake in snakes.Shuffle())
            //{
            //    snake.Move();
            //}
        }
    }
}
