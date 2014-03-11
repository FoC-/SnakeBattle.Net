using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core
{
    public class BattleManager
    {
        private readonly IList<Fighter> fighters;
        private readonly Replay replay;
        private readonly Random random = new Random();

        public BattleManager(IList<Fighter> fighters, Replay replay)
        {
            this.fighters = fighters;
            this.replay = replay;
        }

        public void Fight(int rounds)
        {
            PutFighters();
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

        private void PutFighters()
        {
            var heads = new[]
            {
                new Move {X = 1, Y = 13, Direction = Direction.East},
                new Move {X = 25, Y = 13, Direction = Direction.West},
                new Move {X = 13, Y = 1, Direction = Direction.North},
                new Move {X = 13, Y = 25, Direction = Direction.South}
            };

            // Set heads on gateways
            var n = 0;
            foreach (var fighter in fighters)
                fighter.Head = heads[n++];

            //bite: 9 times empty space in front of head to grow
            foreach (var fighter in fighters)
                for (var i = 0; i < 9; i++)
                    fighter.GrowForward();
        }
    }
}
