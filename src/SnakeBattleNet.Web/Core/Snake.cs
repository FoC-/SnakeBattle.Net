using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Core
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
        public ICollection<IDictionary<Position, ChipCell>> Chips { get; set; }

        private Snake()
        {
            Chips = new List<IDictionary<Position, ChipCell>>();
            Id = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            Created = DateTime.Now;
            Score = 1500;
            Wins = 0;
            Loses = 0;
            Matches = 0;
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
    }
}
