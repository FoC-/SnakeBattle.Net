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

        public VisibleArea GetVisibleAreaFor(BattleSnake snake)
        {
            throw new NotImplementedException();
            //return GetVisibleAreaInternal(snake.SizeOfVisibleArea, GetHeadPosition(snake));
        }

        //private void GetHeadPosition(BattleSnake snake)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
