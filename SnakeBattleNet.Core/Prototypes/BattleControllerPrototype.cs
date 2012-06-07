using System.Collections.Generic;
using System.Dynamic;

namespace SnakeBattleNet.Core.Prototypes
{
    public class BattleControllerPrototype
    {
        public dynamic PlayBattle()
        {
            dynamic repository = new ExpandoObject();

            IBattleField battleField = new BattleField();
            List<Snake> snakes = GetSnakes();

            dynamic battle = new Battle(battleField, snakes); //battle.Create(battleField, snakes);
            dynamic battleReplay = battle.PlayToEnd();

            repository.Save(battle);
            repository.Save(battleReplay);

            return battleReplay.ToJson();
        }
        
        //note restuta: that would be version 2 of the core
        public dynamic PlayBattle2()
        {
            dynamic repository = new ExpandoObject();
            dynamic battleBuilder = new ExpandoObject();

            List<Snake> snakes = GetSnakes();

            dynamic battle = battleBuilder
                .Battle
                    .MaxRounds(500)
                    .FieldSize(25)
                .Snakes
                    .Max(4)
                .BuildWith(snakes);

            //battle = battleFactory.CreateUsing(classicBattleRules);

            battle.PlayToEnd();

            repository.Save(battle);
            repository.Save(battle.Replay); //saving of replay is relevant not for all battles, that's why it shouldn't be saved when battle is saved

            return battle.Replay.ToJson();
        }

        private List<Snake> GetSnakes()
        {
            throw new System.NotImplementedException();
        }
    }
}