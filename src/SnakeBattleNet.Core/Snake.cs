using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Snake
    {
        public string Id { get; private set; }
        public string OwnerId { get; private set; }
        public string Name { get; set; }
        public DateTime Created { get; private set; }
        public int Score { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int Matches { get; private set; }
        public int VisionRadius { get; set; }
        public int ModulesMax { get; set; }
        public ICollection<IDictionary<Position, ChipCell>> Chips { get; set; }

        public int Length { get { return BodyParts.Count; } }
        public LinkedList<Move> BodyParts { get; private set; }

        private Snake()
        {
            Chips = new List<IDictionary<Position, ChipCell>>();
            BodyParts = new LinkedList<Move>();
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            Created = DateTime.Now;
            Score = 1500;
            Wins = 0;
            Loses = 0;
            Matches = 0;
            VisionRadius = 7;
            ModulesMax = 9;
        }

        public Snake(string ownerId)
            : this()
        {
            OwnerId = ownerId;
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
