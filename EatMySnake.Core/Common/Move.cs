﻿using System;
using SnakeBattleNet.Utils.Extensions;

namespace EatMySnake.Core.Common
{
    public class Move : EventArgs
    {
        public readonly int X;
        public readonly int Y;
        public readonly Direction direction;
        public Move(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            this.direction = direction;
        }

        public override int GetHashCode()
        {
            return "{0}{1}".F(X, Y).GetHashCode();
        }

        //restuta->foc: this doesn't include direction comparison, why two Move objects with the same X and Y
        //but different directions should be considered as equal?
        public override bool Equals(object obj)
        {
            Move move = (Move)obj;
            return move.X == X && move.Y == Y;
        }

        public override string ToString()
        {
            return "X={0,2},Y={1,2},Direction is {2,5}".F(X, Y, direction);
        }
    }
}
