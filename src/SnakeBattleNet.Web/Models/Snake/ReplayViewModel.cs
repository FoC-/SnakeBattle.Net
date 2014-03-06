using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ReplayViewModel
    {
        public IEnumerable<ContentViewModel> BattleField { get; set; }
        public IDictionary<int, ICollection<ReplayEvent>> Frames { get; set; }
    }
}