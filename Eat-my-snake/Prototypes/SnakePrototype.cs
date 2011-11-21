using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Prototypes
{
    class SnakePrototype : ISnake
    {
        public int BodyLength
        {
            get { throw new NotImplementedException(); }
        }

        public void Bite(SnakePrototype bitedSnake)
        {
            //fire event: eventBus.Publish(new SnakeBitedEvent(this, bitedSnake))
            bitedSnake.Decrease();
            this.Grow();
        }

        private void Grow()
        {
            throw new NotImplementedException();
        }

        private void Decrease()
        {
            throw new NotImplementedException();
        }
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
        public event EventHandler NextTurn;

        public void OnNextTurn(EventArgs e)
        {
            EventHandler handler = NextTurn;
            if (handler != null) handler(this, e);
        }



    }
}
