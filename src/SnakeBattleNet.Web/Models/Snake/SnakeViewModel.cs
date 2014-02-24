using System;
using System.Collections.Generic;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class SnakeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int Score { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public ICollection<ChipViewModel> Chips { get; set; }

        public SnakeViewModel()
        {
            Chips = new List<ChipViewModel>();
        }
    }
}