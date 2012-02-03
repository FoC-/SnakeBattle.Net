using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Battle;
using EatMySnake.Core.Common;


namespace EatMySnake.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IBattleField battleField = new BattleField();

            var id = Guid.NewGuid();
            var brainChips = new List<Matrix>();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(id, "Snake number 1", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 2", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 3", id, brainChips));
            snakes.Add(new Snake(id, "Snake number 4", id, brainChips));

            BattleManager battleManager = new BattleManager(battleField, snakes);
            for (int i = 0; i < 500000; i++)
            {
                battleManager.Move();
            }
            Console.Read();
        }
    }
}