using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeBattleNet.Core.Prototypes
{

    /*
     * x1 y1 x2 y2
     * 8  8| 8  7| F
     * 
     *  
     * var deltaX = x2 - x1; //0 (0, +1 or -1)
     * var deltaY = y2 - y1; //-1
     * 
     * //combined version will just add or substract "0" in one of the cases for each case
     *      case F:
     *          x1 = x1 - deltaX;
     *          y1 = y1 - deltaY;
     *      case L:
     *          x1 = x1 - deltaY;
     *          y1 = y1 + deltaX;
     *      case R:
     *          x1 = x1 + deltaY;
     *          y1 = y1 - deltaX;
     * 
     * if (deltaX != 0)
     *      case F:
     *          x1 = x1 - deltaX; //x1-- or x1++
     *      case L:
     *          y1 = y1 + deltaX; //y1++ or y--
     *      case R:
     *          y1 = y1 - deltaX; //y1-- or y++
     * else //then deltaY != 0 is true, cos coor1 and coor2 are also direct neighbours
     *      case F:
     *          y1 = y1 - deltaY;
     *      case L:
     *          x1 = x1 - deltaY;
     *      case R:
     *          x1 = x1 + deltaY;
     * 
     * */


    /// <summary>
    /// Represents battle field where snakes are trying to eat each other.
    /// </summary>
    internal class BattleField : IBattleField
    {
        private IList<BattleSnake> snakes;
        private int[] field;

        public void AddSnakes(IList<BattleSnake> snakes)
        {
            this.snakes = snakes;
            foreach (var battleSnake in snakes)
            {
                //battleSnake.PutOnBattleField(this);
            }
        }

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
