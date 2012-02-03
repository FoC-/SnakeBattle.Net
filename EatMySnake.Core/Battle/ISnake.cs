using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public interface ISnake
    {
        event EventHandler Moving;
        event EventHandler Biting;
        event EventHandler Dead;
        Guid Guid { get; }
        string Name { get; }
        Guid Owner { get; }
        List<Matrix> BrainModules { get; }
        int VisionRadius { get; }
        int Length { get; }
        Move GetHeadPosition();
        Move GetTailPosition();
        void NextMove(Move newHeadPosition);
        void Bite(Move newHeadPosition);
        void Bitten();
    }
}