using System;
using System.Collections.Generic;
using System.Linq;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class Snake : ISnake
    {
        public event EventHandler Moving;
        public event EventHandler Biting;
        public event EventHandler Dead;

        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public Guid Owner { get; private set; }

        public List<Matrix> BrainModules { get; private set; }
        public int VisionRadius { get; private set; }
        public int Length
        {
            get
            {
                if (BodyParts.Count == 0)
                    FireDeadEvent();
                return BodyParts.Count;
            }
        }

        private LinkedList<Move> BodyParts;

        public Snake(Guid guid, string name, Guid owner, List<Matrix> brainModules, int visionRadius = 7)
        {
            Guid = guid;
            Name = name;
            Owner = owner;
            BrainModules = brainModules;
            VisionRadius = visionRadius;
            BodyParts = new LinkedList<Move>();
        }

        public Move GetHeadPosition()
        {
            return Length == 0 ? null : BodyParts.First();
        }

        public Move GetTailPosition()
        {
            return Length < 2 ? null : BodyParts.Last();
        }

        public void NextMove(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
            BodyParts.RemoveLast();
            FireMoveEvent(newHeadPosition);
        }

        public void Bite(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
            FireBiteEvent(newHeadPosition);
        }

        public void Bitten()
        {
            if (Length != 0) BodyParts.RemoveLast();
        }

        private void FireDeadEvent()
        {
            if (Dead != null)
                Dead(this, EventArgs.Empty);
        }

        private void FireMoveEvent(EventArgs move)
        {
            if (Moving != null)
                Moving(this, move);
        }

        private void FireBiteEvent(EventArgs move)
        {
            if (Biting != null)
                Biting(this, move);
        }
    }
}
