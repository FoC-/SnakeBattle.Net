using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Replay.GameEvents
{
    public class SnakeMove : GameEvent
    {
        public string Snake { get; set; }
        public int ChipUsed { get; set; }
        public Directed NewHeadPosition { get; set; }
    }
}