using System;
using System.Collections.Generic;
using SnakeBattleNet.ReplayRecorder.Contracts;

namespace SnakeBattleNet.ReplayRecorder
{
    public class ReplayRecorder : IReplayRecorder
    {
        private bool initialized;
        private Replay replay;

        #region Implementation of IReplayRecorder

        public void Initialize(int fieldWidth, int fieldHeight, int randomSeed, IEnumerable<string> fieldObjectsId)
        {
            if (initialized) throw new InvalidOperationException("Replay recorder already initialized");
            replay = new Replay();
            replay.SetFieldWidth(fieldWidth);
            replay.SetFieldHeight(fieldHeight);
            replay.SetRandomSeed(randomSeed);
            replay.SetUniqueToShortIdMap(fieldObjectsId);
            initialized = true;
        }

        public void AddEvent(string objectId, int x, int y, Directed directed, Element element)
        {
            if (!initialized) throw new InvalidOperationException("Replay recorder should been initialized first");
            replay.AddEvent(new ReplayEvent(x, y, directed, element, replay.GetShortFromUniqueId(objectId)));
        }

        public Replay GetReplay()
        {
            if (!initialized) throw new InvalidOperationException("Replay recorder should been initialized first");
            return replay;
        }

        #endregion
    }
}