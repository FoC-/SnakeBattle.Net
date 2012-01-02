using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class BrainChip : Matrix
    {
        private Move _headPosition;
        public Move HeadPosition
        {
            get { return _headPosition; }
            set
            {
                Rows[_headPosition.X, _headPosition.Y] = null;
                Rows[value.X,value.Y] = new Row(Content.OwnHead);
                _headPosition = value;
            } 
        }

        public BrainChip(int sizeX, int sizeY)
            : base(sizeX, sizeY)
        {
            InitilaizeWithHead();
        }

        private void InitilaizeWithHead()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Rows[x, y] = null;
                }
            }
            HeadPosition = new Move(SizeX / 2,SizeY / 2,Direction.North);
            Rows[_headPosition.X, _headPosition.Y] = new Row(Content.OwnHead);
        }
    }
}
