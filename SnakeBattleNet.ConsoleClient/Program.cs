using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Implementation;

namespace SnakeBattleNet.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var battleField = new BattleField();
            var id = Guid.NewGuid().ToString();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(id, "Snake number 0"));
            snakes.Add(new Snake(id, "Snake number 1"));
            snakes.Add(new Snake(id, "Snake number 2"));
            snakes.Add(new Snake(id, "Snake number 3"));

            var replayRecorder = new ReplayRecorder();
            var battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();


            for (int i = 0; i < 3; i++)
            {
                battleManager.Act();
            }
            Console.Read();
        }
    }
}