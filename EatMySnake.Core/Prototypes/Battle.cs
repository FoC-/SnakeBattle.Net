using System;
using System.Collections.Generic;
using System.Linq;

namespace EatMySnake.Core.Prototypes
{
    class Battle
    {
        private readonly dynamic battleField;
        private dynamic battleSnakes;

        public Battle(dynamic battleField, IEnumerable<Snake> snakes)
        {
            this.battleField = battleField;

            battleSnakes = snakes.Select(snake => new BattleSnake(snake));
            this.battleField.AddSnakes(battleSnakes.Shuffle());

            // battleSnakes.ForEach(snake => snake.Died += this.SnakeDiedHandler());
        }

        public void PlayToEnd()
        {
            Start();
        }

        private void Start()
        {
            int turnsMax = 500;

            for (int i = 0; i < turnsMax; i++)
            {
                //todo restuta: consider replace direct method call on events (sound more logical here), e.g. battleField.OnNoSnakeCanBeBitten += bla bla..
                if (battleField.AllSnakesAreStuck() || battleField.NoSnakeCanBeBitten() || battleField.OnlyOneSnakeIsLeft())
                {
                    Finish();
                }

                //CheckIfAnyTailCanBeCut();
                //battleSnakes.Shuffle().ForEach(snake => snake.Move());
            }

            Finish();
        }

        private void Finish()
        {
            throw new NotImplementedException();
        }
    }
}