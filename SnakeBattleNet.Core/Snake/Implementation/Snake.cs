using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core.Implementation
{
    public class Snake : ISnake
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid Owner { get; private set; }

        public IList<IBrainChip> BrainModules { get; private set; }
        public int VisionRadius { get; private set; }
        public int Length
        {
            get { return BodyParts.Count; }
        }

        public LinkedList<Move> BodyParts { get; private set; }

        public Snake(Guid id, Guid owner, string name, IList<IBrainChip> brainModules, int visionRadius = 7)
        {
            Id = id;
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

        public void SetHead(Move head)
        {
            BodyParts.AddFirst(head);
        }

        public void RemoveTail()
        {
            BodyParts.RemoveLast();
        }
    }
}
