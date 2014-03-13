namespace SnakeBattleNet.Core.Contract
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("[{0,2},{1,2}]", X, Y);
        }
    }
}