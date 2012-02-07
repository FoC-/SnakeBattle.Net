using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Prototypes
{
    /// <summary>
    /// Represents battle field where snakes are trying to eat each other.
    /// </summary>
    public class BattleField
    {
        private List<Snake> snakes;
        private int[] field;

        internal VisibleArea GetVisibleAreaFor(BattleSnake snake)
        {
            throw new NotImplementedException();
            //return GetVisibleAreaInternal(snake.SizeOfVisibleArea, GetHeadPosition(snake));
        }

        internal BattleSnake GetTailOwner(BattleSnake snake, MoveDirection moveDirection)
        {
           //var coordinates =  GetRelativeToMoveDirectionCoordinates(snake, moveDirection); //translate relative coordinates to absolute
           //var tailOwner = snakes.Where(snake => snake.Tail.Coordinates == coordinates).SingleOrDefault():
           /* 
            * return tailOwner;
            */

            throw new NotImplementedException();
        }

        //private void GetHeadPosition(BattleSnake snake)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
