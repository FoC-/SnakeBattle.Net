namespace SnakeBattleNet.Core.Contract
{
    public class Cell<TContent> : Position
    {
        public TContent Content { get; set; }
    }
}