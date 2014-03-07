using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Observers
{
    public interface IObserver
    {
        void Notify(Move cell);
    }
}