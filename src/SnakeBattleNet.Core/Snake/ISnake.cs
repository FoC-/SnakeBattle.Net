using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core
{
    public interface ISnake
    {
        string Id { get; }
        string OwnerId { get; }
        string SnakeName { get; set; }
        DateTime Created { get; }
        int Score { get; }
        int Wins { get; }
        int Loses { get; }
        int Matches { get; }
        int VisionRadius { get; set; }
        int ModulesMax { get; set; }
        ICollection<IBrainModule> BrainModules { get; set; }

        void Win();
        void Lose();

        int Length { get; }
        LinkedList<Move> BodyParts { get; }
        Move Head { get; set; }
        Move Tail { get; }
        void CutTail();
    }
}