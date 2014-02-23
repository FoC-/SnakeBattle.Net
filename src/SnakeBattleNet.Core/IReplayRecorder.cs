using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public interface IReplayRecorder
    {
        void Initialize(int fieldWidth, int fieldHeight, int randomSeed, IEnumerable<string> fieldObjectsId);

        void AddEvent(string objectId, int x, int y, Direction directed, Content element);

        Replay GetReplay();
    }
}
