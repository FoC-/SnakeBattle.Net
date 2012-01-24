using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace EatMySnake.Core.Prototypes
{
    class Battle
    {
        public Battle(dynamic battleField, IEnumerable<Snake> snakes)
        {
            dynamic battleSnakes = snakes.Select(snake => new BattleSnake(snake));
            battleField.AddSnakes(battleSnakes.Shuffle());

           // battleSnakes.ForEach(snake => snake.Died += this.SnakeDiedHandler());
        }

        public void PlayToEnd()
        {
            
        }

        /*
                 * new Battle(battleField, snakes){
                 * 
                 *  var battleSnakes = snakes.ForEach(snake => new BattleSnake(snake));
                 *  battleField.AddSnakes(battleSnakes.Shuffle());
                 *  
                 *  battleSnakes.ForEach(snake => snake.Died += this.SnakeDiedHandler());
                 * }
                 * 
                 * Battle.Start(){
                 *  for (int i = 0; i < totalTurns; i++)
                 *  {
                 *    //todo restuta: consider replace direct method call on events (sound more logical here), e.g. battleField.OnNoSnakeCanBeBitten += bla bla..
                 *    if (battleField.AllSnakesAreStuck() || battleField.NoSnakeCanBeBitten() || battleField.OnlyOneSnakeIsLeft())
                 *    {
                 *      Finish();
                 *    }
                 *    
                 *    CheckIfAnyTailCanBeCut();
                 *    battleSnakes.Shuffle().ForEach(snake => snake.Move());
                 *  }
                 * }
                 * */

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BattleControllerPrototype
    {
        public dynamic Index()
        {
            dynamic repository = new ExpandoObject();

            BattleField battleField = new BattleField();
            List<Snake> snakes = GetSnakes();

            dynamic battle = new Battle(battleField, snakes);
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