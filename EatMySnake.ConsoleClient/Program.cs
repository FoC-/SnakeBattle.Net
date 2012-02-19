using System;
using System.Collections.Generic;
using EatMySnake.Core;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlefield.Implementation;
using EatMySnake.Core.Battlemanager;
using EatMySnake.Core.Implementation;
using EatMySnake.Core.Snake;

namespace EatMySnake.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IBattleField battleField = new BattleField();

            var id = Guid.NewGuid();
            var brainChips = new List<IBrainChip>();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(id, id, "Snake number 1", brainChips));
            snakes.Add(new Snake(id, id, "Snake number 2", brainChips));
            snakes.Add(new Snake(id, id, "Snake number 3", brainChips));
            snakes.Add(new Snake(id, id, "Snake number 4", brainChips));

            BattleManager battleManager = new BattleManager(battleField, snakes);
            battleManager.InitializeField();


            for (int i = 0; i < 500; i++)
            {
                battleManager.Act();
            }
            Console.Read();
        }
    }
}