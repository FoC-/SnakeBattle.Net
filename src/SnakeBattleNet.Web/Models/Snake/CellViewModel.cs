using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class CellViewModel
    {
        public Position Position { get; set; }
        public ChipCell Content { get; set; }
    }
}