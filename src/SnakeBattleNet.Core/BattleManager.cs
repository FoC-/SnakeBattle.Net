using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        public void Fight(IList<Fighter> fighters, int rounds)
        {
            var replayRecorder = new ReplayRecorder();
            var battleField = Build.BattleField();
            foreach (var fighter in fighters)
            {
                fighter.Attach(replayRecorder);
                fighter.Attach(battleField);
            }

            InitializeField(battleField, fighters);
            for (int i = 0; i < rounds; i++)
                Act(battleField, fighters);
        }

        private static Move[] CreateGateways()
        {
            const int gatewaysPerSide = 1;

            var moves = new List<Move>();
            const int x = Build.BattleFieldSideLength / (gatewaysPerSide + 1);
            const int y = Build.BattleFieldSideLength / (gatewaysPerSide + 1);

            for (var i = 1; i < gatewaysPerSide + 1; i++)
            {
                moves.Add(new Move(new Position { X = 0, Y = i * y }, Direction.East));
                moves.Add(new Move(new Position { X = Build.BattleFieldSideLength - 1, Y = i * y }, Direction.West));
                moves.Add(new Move(new Position { X = i * x, Y = 0 }, Direction.North));
                moves.Add(new Move(new Position { X = i * x, Y = Build.BattleFieldSideLength - 1 }, Direction.South));
            }
            return moves.ToArray();
        }

        private void InitializeField(View<Content> battleField, IList<Fighter> fighters)
        {
            var gateways = CreateGateways();

            //bite: gateways
            int n = 0;
            foreach (var fighter in fighters)
                fighter.Head = gateways[n++];

            //bite: 10 times empty space in front of head to grow
            foreach (var fighter in fighters)
                for (int i = 0; i < 10; i++)
                    fighter.MoveForward();

            // remove tails
            foreach (var fighter in fighters)
                fighter.CutTail();

            foreach (var gateway in gateways)
            {
                battleField[gateway.Position] = Content.Wall;
            }
        }

        private void Act(View<Content> battleField, IEnumerable<Fighter> fighters)
        {
            // use something like builder 
            foreach (var snake in fighters.Shuffle())
            {
                var possibleMoves = battleField.PossibleMoves(snake);
                var move = possibleMoves[new Random().Next(possibleMoves.Length)];
                if (move == null) continue;
                snake.TryToBite(fighters, move);
            }
        }
    }
}
