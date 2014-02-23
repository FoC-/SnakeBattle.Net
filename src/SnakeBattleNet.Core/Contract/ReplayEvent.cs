namespace SnakeBattleNet.Core.Contract
{
    public class ReplayEvent
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction D { get; private set; }
        public Content E { get; private set; }
        public int I { get; private set; }

        //for serialization
        private ReplayEvent() { }
        internal ReplayEvent(int x, int y, Direction directed, Content element, int shortId)
        {
            X = x;
            Y = y;
            D = directed;
            E = element;
            I = shortId;
        }

        public override string ToString()
        {
            return string.Format("ID:[{0}];Position:[{1}:{2}];Looking:[{3}]", I, X, Y, D);
        }
    }
}