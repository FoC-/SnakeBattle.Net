using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ChipCellViewModel : ContentViewModel
    {
        public bool Exclude { get; set; }
        public Color Color { get; set; }
        public bool IsSelf { get; set; }
    }
}