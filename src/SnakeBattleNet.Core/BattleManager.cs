using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        public Replay Fight(IList<Fighter> fighters, int rounds)
        {
            var random = new Random();
            var battleField = BattleField.Build();
            var replay = new Replay { BattleField = battleField.ToDictionary(k => k.Key, v => v.Value) };

            foreach (var fighter in fighters)
            {
                fighter.Attach(battleField);
            }

            var gateways = CreateGateways();
            PutFighters(gateways, fighters);
            foreach (var gateway in gateways)
            {
                battleField[gateway] = Content.Wall;
            }

            for (var round = 0; round < rounds; round++)
            {
                foreach (var fighter in fighters.Shuffle())
                {
                    var possibleMoves = battleField.PossibleMoves(fighter);
                    if (possibleMoves.Length == 0) continue;
                    var move = possibleMoves[random.Next(possibleMoves.Length)];
                    fighter.BiteMove(fighters, move);
                }
                foreach (var fighter in fighters)
                {
                    replay.SaveFighter(round, fighter);
                }
            }
            return replay;
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
                moves.Add(new Move { X = 0, Y = i * y, Direction = Direction.East });
                moves.Add(new Move { X = BattleField.SideLength - 1, Y = i * y, Direction = Direction.West });
                moves.Add(new Move { X = i * x, Y = 0, Direction = Direction.North });
                moves.Add(new Move { X = i * x, Y = BattleField.SideLength - 1, Direction = Direction.South });
            }
            return moves;
        }
    }
}
