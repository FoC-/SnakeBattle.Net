using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Extensions;

namespace EatMySnake.Core.Prototypes
{

    class Battle
    {
        /*
                 * new Battle(battleField, snakes){
                 *  var battleSnakes = snakes.ForEach(snake => new BattleSnake(snake));
                 *  battleField.AddSnakes(battleSnakes.Shuffle());
                 *  
                 *  battleSnakes.ForEach(snake => snake.Died += this.SnakeDiedHandler());
                 * }
                 * 
                 * Battle.Start(){
                 *  for (int i = 0; i < totalTurns; i++)
                 *  {
                 *    if (AllSnakesAreStuck() || NoSnakeCanBeBitten() || OnlyOneSnakeIsLeft())
                 *    {
                 *      Finish();
                 *    }
                 *    
                 *    CheckIfAnyTailCanBeCut();
                 *    battleSnakes.Shuffle().ForEach(snake => snake.Move());
                 *  }
                 * }
                 * */
    }


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

    internal class LogicalChip
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
