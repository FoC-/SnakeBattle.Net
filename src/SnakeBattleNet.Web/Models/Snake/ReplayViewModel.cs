using System.Collections.Generic;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ReplayViewModel
    {
        public IEnumerable<IEnumerable<dynamic>> Frames { get; set; }
    }
}