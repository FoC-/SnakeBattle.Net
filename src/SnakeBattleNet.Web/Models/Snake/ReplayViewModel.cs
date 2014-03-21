using System.Collections.Generic;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ReplayViewModel
    {
        public int RandomSeed { get; set; }
        public IEnumerable<ContentViewModel> BattleField { get; set; }
        public IDictionary<int, IDictionary<string, IEnumerable<ContentViewModel>>> Frames { get; set; }
    }
}