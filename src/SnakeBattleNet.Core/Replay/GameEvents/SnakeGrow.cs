using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Replay.GameEvents
{
    public class SnakeGrow : GameEvent
    {
        public string Snake { get; set; }
        public Directed NewHeadPosition { get; set; }
    }
}