namespace SnakeBattleNet.ReplayRecorder.Contracts
{
    public class ReplayEvent
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Directed D { get; private set; }
        public Element E { get; private set; }
        public int I { get; private set; }

        //for serialization
        private ReplayEvent() { }
        internal ReplayEvent(int x, int y, Directed direction, Element element, int shortId)
        {
            X = x;
            Y = y;
            D = direction;
            E = element;
            I = shortId;
        }
    }
}