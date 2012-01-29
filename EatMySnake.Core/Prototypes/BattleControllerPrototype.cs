using System.Collections.Generic;
using System.Dynamic;

namespace EatMySnake.Core.Prototypes
{
    public class BattleControllerPrototype
    {
        public dynamic PlayBattle()
        {
            dynamic repository = new ExpandoObject();

            BattleField battleField = new BattleField(/* size:9*/);
            List<Snake> snakes = GetSnakes();

            dynamic battle = new Battle(battleField, snakes); //battle.Create(battleField, snakes);
            battle.PlayToEnd();

            repository.Save(battle);
            repository.SaveReplay(battle.Replay);

            return battle.Replay.ToJson();
        }

        public dynamic PlayBattle2()
        {
            dynamic repository = new ExpandoObject();
            dynamic battleBuilder = new ExpandoObject();

            List<Snake> snakes = GetSnakes();

            dynamic battle = battleBuilder
                .Battle
                    .MaxRounds(500)
                    .BattleFieldSize(17)
                .Snakes
                    .Max(4)
                .Build();

            battle = battleFactory.CreateUsing(classicBattleSpecification);

            
            battle.PlayToEnd();

            repository.Save(battle);
            repository.SaveReplay(battle.Replay);

            return battle.Replay.ToJson();
        }

        private List<Snake> GetSnakes()
        {
            throw new System.NotImplementedException();
        }
    }
}