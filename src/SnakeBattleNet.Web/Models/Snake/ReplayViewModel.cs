using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ReplayViewModel
    {
        public IEnumerable<ContentViewModel> BattleField { get; set; }
        public IDictionary<int, IDictionary<string, IEnumerable<Move>>> Frames { get; set; }
    }
}