using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class BattleManager
    {
        private readonly Replay replay;
        private readonly Move[] gateways = CreateGateways();

        public BattleManager(Replay replay)
        {
            this.replay = replay;
        }

        public void Fight(View<Content> battleField, IList<Fighter> fighters, int rounds)
        {
            if (fighters.Count > gateways.Length)
                throw new Exception("Number of fighters is more then gateways");

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
            PutEmptySpaceAndWalls(battleField);

            //bite: gateways
            int n = 0;
            foreach (var snake in fighters)
                SnakeIsGrowing(battleField, snake, gateways[n++]);

            //bite: 10 times empty space in front of head to grow
            foreach (var snake in fighters)
                for (int i = 0; i < 10; i++)
                    SnakeIsGrowing(battleField, snake, StraitMove(snake.Head));

            // remove tails
            foreach (var snake in fighters)
                CutTail(battleField, snake);

            PutWallsOnGateways(battleField);
        }

        private void Act(View<Content> battleField, IEnumerable<Fighter> fighters)
        {
            // use something like builder 
            foreach (var snake in fighters.Shuffle())
            {
                //Try to move according brain chip
                var possibleMoves = battleField.PossibleMoves(snake);
                var move = possibleMoves[new Random().Next(possibleMoves.Length)];
                if (move == null) continue;
                TryToBite(battleField, snake, fighters, move);
            }
        }

        private Move StraitMove(Move head)
        {
            switch (head.Direction)
            {
                case Direction.North:
                    return Move.ToNothFrom(head.Position);
                case Direction.West:
                    return Move.ToWestFrom(head.Position);
                case Direction.East:
                    return Move.ToEastFrom(head.Position);
                case Direction.South:
                    return Move.ToSouthFrom(head.Position);
                default:
                    throw new Exception("Something wrong in moving strait!");
            }
        }

        private void TryToBite(View<Content> battleField, Fighter snakeBiter, IEnumerable<Fighter> fighters, Move newHeadPosition)
        {
            foreach (var snake in fighters)
            {
                if (snakeBiter.Head.Equals(snake.Tail))
                {
                    if (snakeBiter.Id == snake.Id)
                        SnakeIsMoving(battleField, snakeBiter, newHeadPosition);
                    else
                        SnakeIsbitting(battleField, snakeBiter, snake);
                    return;
                }
            }
            SnakeIsMoving(battleField, snakeBiter, newHeadPosition);
        }

        private void PutEmptySpaceAndWalls(View<Content> battleField)
        {
            for (int x = 0; x < Build.BattleFieldSideLength; x++)
                for (int y = 0; y < Build.BattleFieldSideLength; y++)
                {
                    var position = new Position { X = x, Y = y };
                    if (x == 0 || y == 0 || x == Build.BattleFieldSideLength - 1 || y == Build.BattleFieldSideLength - 1)
                    {
                        battleField[position] = Content.Wall;
                        var id = replay.GetShortId("View.Id");
                        replay.AddEvent(new ReplayEvent(id, position, Content.Wall, Direction.North));
                    }
                    else
                    {
                        battleField[position] = Content.Empty;
                        var id = replay.GetShortId("View.Id");
                        replay.AddEvent(new ReplayEvent(id, position, Content.Empty, Direction.North));
                    }
                }
        }

        private void SnakeIsMoving(View<Content> battleField, Fighter fighter, Move newHeadPosition)
        {
            CutTail(battleField, fighter);
            MoveHead(battleField, fighter, newHeadPosition);
        }

        private void SnakeIsbitting(View<Content> battleField, Fighter snakeBitter, Fighter snakeBitten)
        {
            Move tailPosition = snakeBitten.Tail;
            CutTail(battleField, snakeBitten);
            MoveHead(battleField, snakeBitter, tailPosition);
        }

        private void SnakeIsGrowing(View<Content> battleField, Fighter snake, Move newHeadPosition)
        {
            MoveHead(battleField, snake, newHeadPosition);
        }

        private void MoveHead(View<Content> battleField, Fighter fighter, Move newHeadPosition)
        {
            if (fighter.Length != 0)
            {
                // replace old head with body
                battleField[fighter.Head.Position] = Content.Body;
                replay.AddEvent(new ReplayEvent(replay.GetShortId(fighter.Id), fighter.Head.Position, Content.Body, fighter.Head.Direction));
            }
            // add head to fighter
            fighter.Head = newHeadPosition;
            // put new head
            battleField[fighter.Head.Position] = Content.Head;
            replay.AddEvent(new ReplayEvent(replay.GetShortId(fighter.Id), fighter.Head.Position, Content.Head, fighter.Head.Direction));
        }

        private void CutTail(View<Content> battleField, Fighter fighter)
        {
            // replace old tail with empty row
            battleField[fighter.Tail.Position] = Content.Empty;
            replay.AddEvent(new ReplayEvent(replay.GetShortId("View.Id"), fighter.Tail.Position, Content.Empty, fighter.Tail.Direction));

            // remove tail from fighter
            fighter.CutTail();

            // put new tail on field
            battleField[fighter.Tail.Position] = Content.Tail;
            replay.AddEvent(new ReplayEvent(replay.GetShortId(fighter.Id), fighter.Tail.Position, Content.Tail, fighter.Tail.Direction));
        }

        private void PutWallsOnGateways(View<Content> battleField)
        {
            foreach (var gateway in gateways)
            {
                battleField[gateway.Position] = Content.Wall;
                replay.AddEvent(new ReplayEvent(replay.GetShortId("View.Id"), gateway.Position, Content.Gateway, gateway.Direction));
            }
        }
    }
}
