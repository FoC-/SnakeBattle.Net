namespace SnakeBattleNet.Core.Replay.GameEvents
{
    public class GameInit : GameEvent
    {
        public int RandomSeed { get; set; }
        public BattleField BattleField { get; set; }
    }
}