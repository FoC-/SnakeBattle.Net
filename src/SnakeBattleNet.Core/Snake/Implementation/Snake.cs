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
        public string SnakeName { get; set; }
        public DateTime Created { get; private set; }
        public int Score { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int Matches { get; private set; }
        public int VisionRadius { get; set; }
        public int ModulesMax { get; set; }
        public ICollection<IBrainModule> BrainModules { get; set; }

        public int Length { get { return BodyParts.Count; } }
        public LinkedList<Move> BodyParts { get; private set; }

        private Snake()
        {
            BrainModules = new List<IBrainModule>();
            BodyParts = new LinkedList<Move>();
        }

        public Snake(string id, string ownerId)
            : this()
        {
            Id = id;
            OwnerId = ownerId;
            Created = DateTime.Now;

            Score = 1500;
            Wins = 0;
            Loses = 0;
            Matches = 0;
            VisionRadius = 7;
            ModulesMax = 9;
        }

        public void Win()
        {
            Wins++;
            Score += 5;
            Matches++;
        }

        public void Lose()
        {
            Loses++;
            Score -= 5;
            Matches++;
        }

        public Move Head
        {
            get { return Length == 0 ? null : BodyParts.First(); }
            set { BodyParts.AddFirst(value); }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            BodyParts.RemoveLast();
        }
    }
}
