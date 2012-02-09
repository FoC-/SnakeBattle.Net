using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlefield.Implementation;
using EatMySnake.Core.Battlemanager;
using EatMySnake.Core.Snake;
using EatMySnake.Core.Snake.Implementation;


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
            snakes.Add(new Snake(id, "Snake number 1", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 2", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 3", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 4", id, brainChips));

            BattleManager battleManager = new BattleManager(battleField, snakes);
            battleManager.SetupHandlers();
            battleManager.InitializeField();


            for (int i = 0; i < 500; i++)
            {
                battleManager.Act();
            }
            Console.Read();
        }
    }
}