using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core
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