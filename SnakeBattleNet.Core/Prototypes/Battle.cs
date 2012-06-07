using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Core.Prototypes
{
    internal class Battle : IBattle
    {
        private readonly dynamic battleField;
        private dynamic battleSnakes;

        public int FieldSize { get; private set; }
        public int MaxRounds { get; private set; }

        public Battle(int maxRounds, int fieldSize)
        {
            FieldSize = fieldSize;
            MaxRounds = maxRounds;
        }

        public Battle()
        {
            FieldSize = 25;
            MaxRounds = 500;
        }

        public Battle(IBattleField battleField, IEnumerable<Snake> snakes) 
            : this()
        {
            this.battleField = battleField;

            battleSnakes = snakes.Select(snake => new BattleSnake(snake, new DummyMind()));
            //set snakes starting coordinates here
            this.battleField.AddSnakes(battleSnakes.Shuffle());

            // battleSnakes.ForEach(snake => snake.Died += this.SnakeDiedHandler());
        }

        public void PlayToEnd()
        {
            Start();
        }

        private void Start()
        {
            int roundsMax = 500;

            for (int i = 0; i < roundsMax; i++)
            {
                //todo restuta: consider replace direct method call on events (sound more logical here), e.g. battleField.OnNoSnakeCanBeBitten += bla bla..
                if (battleField.AllSnakesAreStuck() || battleField.NoSnakeCanBeBitten() || battleField.OnlyOneSnakeIsLeft())
                {
                    Finish();
                }

                //battleSnakes.Shuffle().ForEach(snake => snake.Move());
            }

            Finish();
        }

        private void Finish()
        {
            throw new NotImplementedException();
        }

        public BattleReplay Replay
        {
            get { throw new NotImplementedException(); }
        }
    }
}