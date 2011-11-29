using System;
using System.Collections.Generic;
using System.Linq;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class Snake
    {
        public string Name { get; private set; }
        public int VisionRadius { get; private set; }
        public Guid Uid { get; private set; }

        public List<Matrix> BrainModules = new List<Matrix>();
        LinkedList<Move> BodyParts = new LinkedList<Move>();

        public Snake()
        {
            VisionRadius = 7;
            Name = Guid.NewGuid().ToString();
        }

        public Snake LoadSnake(Guid uid)
        {
            Uid = uid;
            Name = Guid.NewGuid().ToString();
            return this;
        }

        public bool SaveSnake()
        {
            return true;
        }

        public Move GetHeadPosition()
        {
            return BodyParts.First();
        }

        public Move GetTailPosition()
        {
            return BodyParts.Last();
        }

        public void NextMove(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
            BodyParts.RemoveLast();
        }

        public void Bite(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
        }

        public void Bitten()
        {
            BodyParts.RemoveLast();
        }
    }
}
