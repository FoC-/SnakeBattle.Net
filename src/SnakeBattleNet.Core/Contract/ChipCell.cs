namespace SnakeBattleNet.Core.Contract
{
    public class ChipCell
    {
        public Position Position { get; set; }
        public bool Exclude { get; set; }
        public Color Color { get; set; }
        public Content Content { get; set; }
        public bool IsSelf { get; set; }
    }
}