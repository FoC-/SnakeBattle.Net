using System.Collections.Generic;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core
{
    public interface ISnake
    {
        string Id { get; }
        string OwnerId { get; }
        string SnakeName { get; }
        int Score { get; }
        int Wins { get; }
        int Loses { get; }
        int VisionRadius { get; }
        int ModulesMax { get; }
        IList<IBrainModule> BrainModules { get; }

        int Length { get; }
        LinkedList<Move> BodyParts { get; }

        void SetName(string name);
        void SetScore(int score);
        void SetWins(int wins);
        void SetLoses(int loses);
        void SetVisionRadius(int radius);
        void SetModulesMax(int modulesMax);
        void InsertModule(int position, IBrainModule brainModule);
        void UpdateModule(IBrainModule brainModule);
        void DeleteModule(string id);

        Move GetHeadPosition();
        Move GetTailPosition();
        void SetHead(Move head);
        void RemoveTail();
    }
}