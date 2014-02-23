namespace SnakeBattleNet.Core.Contract
{
    public class ReplayEvent
    {
        public int I { get; private set; }
        public Position P { get; private set; }
        public Content C { get; private set; }
        public Direction D { get; private set; }

        //for serialization
        private ReplayEvent() { }
        internal ReplayEvent(int id, Position position, Content content, Direction direction)
        {
            I = id;
            P = position;
            D = direction;
            C = content;
        }

        public override string ToString()
        {
            return string.Format("ID:[{0}];Position:{1};Looking:[{2}]", I, P, D);
        }
    }
}