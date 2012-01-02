using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Extensions;

namespace EatMySnake.Core.Prototypes
{
    class BattleManager
    {
        private List<SnakePrototype> snakes;

        public void StartBattle()
        {
            //Battle.NextMove();
            foreach(var snake in snakes.Shuffle())
            {
                /* Move nextMove = snake.GetIntentedNextMove(); //gets move that snake is going to make
                 * if (nextMove.IsAllowed())
                 * {
                 *   MoveSnake(nextMove, snake); 
                 *   or
                 *   snake.Move();
                 * }
                 * else
                 * {
                 * do nothing?
                 * }
                */
            }
        }
    } 


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
            //fire event: eventBus.Publish(new SnakeBittenEvent(this, snakeToBite))
            snakeToBite.Shorten();
            this.Lengthen();
        }

        private void Lengthen()
        {
            throw new NotImplementedException();
        }
            
        private void Shorten()
        {
            /*
             * body.ShortenTail();
             * 
             * if (body.Length == 0)
             *   Die();
             */

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

        //or it is better to have public event and private Die() method
        public void Die()
        {
            
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
