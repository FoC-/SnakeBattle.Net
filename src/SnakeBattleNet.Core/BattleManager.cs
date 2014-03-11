using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        private readonly BattleField battleField;
        private readonly IList<Fighter> fighters;
        private readonly Replay replay;
        private readonly Random random = new Random();

        public BattleManager(BattleField battleField, IList<Fighter> fighters, Replay replay)
        {
            this.battleField = battleField;
            this.fighters = fighters;
            this.replay = replay;
        }

        public void Fight(int rounds)
        {
            var gateways = CreateGateways();
            PutFighters(gateways, fighters);
            foreach (var gateway in gateways)
            {
                battleField[gateway.X, gateway.Y] = gateway.Content;
            }

            for (var round = 0; round < rounds; round++)
            {
                foreach (var fighter in fighters.Shuffle())
                {
                    var possibleMoves = fighter.PossibleMoves();
                    if (possibleMoves.Length == 0) continue;
                    var move = possibleMoves[random.Next(possibleMoves.Length)];
                    fighter.BiteMove(fighters, move);
                }
                foreach (var fighter in fighters)
                {
                    replay.SaveFighter(round, fighter);
                }
            }
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

        private IList<Move> CreateGateways()
        {
            const int gatewaysPerSide = 1;

            var moves = new List<Move>();
            var x = battleField.SideLength / (gatewaysPerSide + 1);
            var y = battleField.SideLength / (gatewaysPerSide + 1);

            for (var i = 1; i < gatewaysPerSide + 1; i++)
            {
                moves.Add(new Move { Content = Content.Wall, X = 0, Y = i * y, Direction = Direction.East });
                moves.Add(new Move { Content = Content.Wall, X = battleField.SideLength - 1, Y = i * y, Direction = Direction.West });
                moves.Add(new Move { Content = Content.Wall, X = i * x, Y = 0, Direction = Direction.North });
                moves.Add(new Move { Content = Content.Wall, X = i * x, Y = battleField.SideLength - 1, Direction = Direction.South });
            }
            return moves;
        }
    }
}
