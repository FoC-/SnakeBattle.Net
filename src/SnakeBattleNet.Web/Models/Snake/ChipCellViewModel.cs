namespace SnakeBattleNet.Web.Models.Snake
{
    public class ChipCellViewModel : ContentViewModel
    {
        public bool Exclude { get; set; }
        public string Color { get; set; }
        public bool IsSelf { get; set; }
    }
}