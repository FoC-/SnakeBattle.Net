using System;
using System.Collections.Generic;
using System.Linq;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class Snake
    {
        public event EventHandler Biting;
        public event EventHandler Moving;

        public string Name { get; private set; }
        public int VisionRadius { get; private set; }
        public int Length
        {
            get { return BodyParts.Count; }
        }
        public Guid Uid { get; private set; }

        public List<Matrix> BrainModules = new List<Matrix>();
        LinkedList<Move> BodyParts = new LinkedList<Move>();

        public Snake()
        {
            VisionRadius = 7;
            Name = Guid.NewGuid().ToString();
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
            FireMoveEvent(newHeadPosition);
            BodyParts.AddFirst(newHeadPosition);
            BodyParts.RemoveLast();
        }

        public void Bite(Move newHeadPosition)
        {
            FireBiteEvent(newHeadPosition);
            BodyParts.AddFirst(newHeadPosition);
        }

        public void Bitten()
        {
            BodyParts.RemoveLast();
        }

        private void FireMoveEvent(Move move)
        {
            if (Moving != null)
                Moving(this, move);
        }

        private void FireBiteEvent(Move move)
        {
            if (Biting != null)
                Biting(this, move);
        }
    }
}
