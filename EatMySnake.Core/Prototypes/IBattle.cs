namespace SnakeBattleNet.Core.Prototypes
{
    public interface IBattle
    {
        void PlayToEnd();
        BattleReplay Replay { get; }
    }
}