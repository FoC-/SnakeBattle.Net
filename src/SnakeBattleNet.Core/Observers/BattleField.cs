using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Observers
{
    public class BattleField : View<Content>, IObserver
    {
        public void Notify(ReplayEvent replayEvent)
        {
            this[replayEvent.P] = replayEvent.C;
        }
    }
}