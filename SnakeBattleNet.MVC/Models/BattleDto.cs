using System.Collections.Generic;

namespace SnakeBattleNet.MVC.Models
{
    public class BattleDto
    {
        public string BattleFieldJson { get; set; }
        public IEnumerable<string> TexturesForSnakes { get; set; }
    }
}