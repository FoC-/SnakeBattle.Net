using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            IBattleField battleField = new BattleField();

            var id = Guid.NewGuid();
            var brainChips = new List<IBrainChip>();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(Guid.NewGuid(), id, "Snake number 0", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), id, "Snake number 1", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), id, "Snake number 2", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), id, "Snake number 3", brainChips));

            IReplayRecorder replayRecorder = new ReplayRecorder();
            BattleManager battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();


            for (int i = 0; i < 3; i++)
            {
                battleManager.Act();
            }
            var replay = replayRecorder.GetReplay();
            //Console.Write(replay.Length);
            Console.Read();
        }
    }
}