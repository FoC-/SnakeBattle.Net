using System.Collections.Generic;

namespace SnakeBattleNet.Web.Models
{
    public class TrainingViewModel
    {
        public bool CanAdd { get; set; }
        public IEnumerable<SnakeStatsViewModel> SnakeStats { get; set; }
    }
}