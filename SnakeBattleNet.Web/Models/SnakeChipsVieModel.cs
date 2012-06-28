using System.Collections.Generic;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Web.Models
{
    public class SnakeBrainModulesVieModel : SnakeViewModelBase
    {
        public IEnumerable<IBrainModule> modules { get; set; }  

        public SnakeBrainModulesVieModel()
        {
        }

        public SnakeBrainModulesVieModel(string id, string name)
            : base(id, name)
        {
        }
    }
}