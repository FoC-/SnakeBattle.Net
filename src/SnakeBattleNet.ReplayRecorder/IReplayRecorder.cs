using System.Collections.Generic;
using SnakeBattleNet.ReplayRecorder.Contracts;

namespace SnakeBattleNet.ReplayRecorder
{
    public interface IReplayRecorder
    {
        void Initialize(int fieldWidth, int fieldHeight, int randomSeed, IEnumerable<string> fieldObjectsId);

        void AddEvent(string objectId, int x, int y, Directed directed, Element element);

        Replay GetReplay();
    }
}
