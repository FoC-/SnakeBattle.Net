using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        public Replay Fight(IList<Fighter> fighters, int rounds)
        {
            var replayRecorder = new ReplayRecorder();
            var battleField = BattleField.Build();
            replayRecorder.SetBattlefield(battleField);

            foreach (var fighter in fighters)
            {
                fighter.Attach(replayRecorder);
                fighter.Attach(battleField);
            }

            var gateways = CreateGateways();
            PutFighters(gateways, fighters);
            foreach (var gateway in gateways)
            {
                battleField[gateway.Position] = Content.Wall;
            }

            for (var round = 0; round < rounds; round++)
            {
                replayRecorder.SetFrameIndex(rounds);
                foreach (var snake in fighters.Shuffle())
                {
                    var possibleMoves = battleField.PossibleMoves(snake);
                    var move = possibleMoves[new Random().Next(possibleMoves.Length)];
                    if (move == null) continue;
                    snake.BiteMove(fighters, move);
                }
            }
            return replayRecorder.Replay();
        }

        private static void PutFighters(IList<Move> gateways, IList<Fighter> fighters)
        {
            // Set heads on gateways
            int n = 0;
            foreach (var fighter in fighters)
                fighter.Head = gateways[n++];

            //bite: 10 times empty space in front of head to grow
            foreach (var fighter in fighters)
                for (int i = 0; i < 10; i++)
                    fighter.GrowForward();

            // remove tails
            foreach (var fighter in fighters)
                fighter.CutTail();
        }

        private static IList<Move> CreateGateways()
        {
            const int gatewaysPerSide = 1;

            var moves = new List<Move>();
            const int x = BattleField.SideLength / (gatewaysPerSide + 1);
            const int y = BattleField.SideLength / (gatewaysPerSide + 1);

            for (var i = 1; i < gatewaysPerSide + 1; i++)
            {
                moves.Add(new Move(new Position { X = 0, Y = i * y }, Direction.East));
                moves.Add(new Move(new Position { X = BattleField.SideLength - 1, Y = i * y }, Direction.West));
                moves.Add(new Move(new Position { X = i * x, Y = 0 }, Direction.North));
                moves.Add(new Move(new Position { X = i * x, Y = BattleField.SideLength - 1 }, Direction.South));
            }
            return moves;
        }
    }
}
