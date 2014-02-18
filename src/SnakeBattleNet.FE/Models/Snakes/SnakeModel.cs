using System;

namespace SnakeBattleNet.FE.Models.Snakes
{
    public class SnakeModel
    {
        public string Id { get; set; }
        public string SnakeName { get; set; }
        public DateTime Created { get; set; }
        public int Score { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Matches { get; set; }
    }
}