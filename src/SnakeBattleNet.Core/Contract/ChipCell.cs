namespace SnakeBattleNet.Core.Contract
{
    public class ChipCell : Cell<Content>
    {
        public bool Exclude { get; set; }
        public Color Color { get; set; }
        public bool IsSelf { get; set; }
    }
}