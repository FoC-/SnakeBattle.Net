namespace SnakeBattleNet.Core.Contract
{
    public class ReplayEvent
    {
        public string Id { get; private set; }
        public Position P { get; set; }
        public Content C { get; private set; }
        public Direction D { get; private set; }

        //for serialization
        private ReplayEvent() { }
        internal ReplayEvent(string id, Position position, Content content, Direction direction)
        {
            P = position;
            D = direction;
            C = content;
            Id = id;
        }

        public override string ToString()
        {
            return string.Format("ID:[{0}];Position:{1};Looking:[{2}]", Id, P, D);
        }
    }
}