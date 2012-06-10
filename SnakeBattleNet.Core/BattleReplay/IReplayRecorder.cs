using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Battlefield;

namespace SnakeBattleNet.Core.BattleReplay
{
    public interface IReplayRecorder
    {
        void InitBattleField(IBattleField battleField);
        void InitSeed(int randomSeed);
        void InitSnakes(IEnumerable<ISnake> snakes);

        void AddEvent(string id, int x, int y, Command command);

        Dictionary<string, object> GetReplay();
    }
}
