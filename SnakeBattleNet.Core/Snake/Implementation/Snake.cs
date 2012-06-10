using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Core.Implementation
{
    public class Snake : ISnake
    {
        public string Id { get; private set; }
        public string OwnerId { get; private set; }
        public string SnakeName { get; private set; }
        public int Score { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int VisionRadius { get; private set; }
        public int ModulesMax { get; private set; }
        public IList<IBrainModule> BrainModules { get; private set; }

        public int Length { get { return BodyParts.Count; } }
        public LinkedList<Move> BodyParts { get; private set; }

        public Snake(string id, string owner)
        {
            Id = id;
            OwnerId = owner;

            BrainModules = new List<IBrainModule>();
            BodyParts = new LinkedList<Move>();
        }

        public void SetName(string name)
        {
            SnakeName = name;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void SetWins(int wins)
        {
            Wins = wins;
        }

        public void SetLoses(int loses)
        {
            Loses = loses;
        }

        public void SetVisionRadius(int radius)
        {
            VisionRadius = radius;
        }

        public void SetModulesMax(int modulesMax)
        {
            ModulesMax = modulesMax;
        }

        public void InsertModule(int position, IBrainModule brainModule)
        {
            if (BrainModules.Count < ModulesMax)
                BrainModules.Insert(position, brainModule);
            else
                throw new ArgumentOutOfRangeException("Maximum of modules is reached.");
        }

        public void UpdateModule(IBrainModule brainModule)
        {
            var tModule = BrainModules.Single(module => module.Id == brainModule.Id);
            var pos = BrainModules.IndexOf(tModule);
            BrainModules.RemoveAt(pos);
            BrainModules.Insert(pos, brainModule);
        }

        public void DeleteModule(string id)
        {
            BrainModules.Remove(BrainModules.Single(module => module.Id == id));
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
