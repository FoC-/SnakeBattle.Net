namespace SnakeBattleNet.Core.Contract
{
    public class LongShortIdPair
    {
        public string L { get; private set; }
        public int S { get; private set; }
        public LongShortIdPair(string longId, int shortId)
        {
            L = longId;
            S = shortId;
        }
    }
}