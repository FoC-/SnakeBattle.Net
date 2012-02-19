using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;

namespace EatMySnake.Core
{
    public interface ISnake
    {
        Guid Id { get; }
        string Name { get; }
        Guid Owner { get; }
        IList<IBrainChip> BrainModules { get; }
        int VisionRadius { get; }
        int Length { get; }
        LinkedList<Move> BodyParts { get; }
        Move GetHeadPosition();
        Move GetTailPosition();
        void SetHead(Move head);
        void RemoveTail();
    }
}