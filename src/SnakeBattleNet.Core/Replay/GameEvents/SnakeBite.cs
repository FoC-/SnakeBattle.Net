namespace SnakeBattleNet.Core.Replay.GameEvents
{
    public class SnakeBite : GameEvent
    {
        public string Snake { get; set; }
        public int ChipUsed { get; set; }
        public string TargetSnake { get; set; }
    }
}