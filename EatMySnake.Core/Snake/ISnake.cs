using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Snake
{
    public interface ISnake
    {
        event EventHandler Moving;
        event EventHandler Biting;
        event EventHandler Dead;
        Guid Guid { get; }
        string Name { get; }
        Guid Owner { get; }
        IList<IBrainChip> BrainModules { get; }
        int VisionRadius { get; }
        int Length { get; }
        Move GetHeadPosition();
        Move GetTailPosition();
        void NextMove(Move newHeadPosition);
        void Bite(Move newHeadPosition);
        void Bitten();
    }
}