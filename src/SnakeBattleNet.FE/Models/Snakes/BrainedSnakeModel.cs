using System.Collections.Generic;

namespace SnakeBattleNet.FE.Models.Snakes
{
    public class BrainedSnakeModel : SnakeModel
    {
        public int VisionRadius { get; set; }
        public int ModulesMax { get; set; }
        public List<BrainModuleModel> BrainModuleModels { get; set; }

        public BrainedSnakeModel()
        {
            BrainModuleModels = new List<BrainModuleModel>();
        }
    }
}