using System.Collections.Generic;

namespace SnakeBattleNet.Web.Models
{
    public class SnakeBrainModulesVieModel : SnakeViewModelBase
    {
        public IEnumerable<string> Ids { get; set; }
        public int Maximum { get; set; }

        public SnakeBrainModulesVieModel(string id, string name)
            : base(id, name)
        {
        }
    }
}