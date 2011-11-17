using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Snake;

namespace EatMySnake.Core.Logic
{
    struct Move
    {
        public int x;
        public int y;
        public Direction direction;
        public Move(int x, int y, Direction direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
        }
    }
}
