﻿using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class BattleManager
    {
        private readonly IReplayRecorder recorder;
        private readonly Move[] gateways = CreateGateways();

        public BattleManager(IReplayRecorder recorder)
        {
            this.recorder = recorder;
        }

        public void Fight(View<Content> battleField, IList<Fighter> fighters, int rounds)
        {
            if (fighters.Count > gateways.Length)
                throw new Exception("Number of fighters is more then gateways");

            int randomSeed = Environment.TickCount;
            recorder.Initialize(Build.BattleFieldSideLength, Build.BattleFieldSideLength, randomSeed, fighters.Select(_ => _.Id).Concat(new[] { "View.Id" }));

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
                    if (x == 0 || y == 0 || x == Build.BattleFieldSideLength - 1 || y == Build.BattleFieldSideLength - 1)
                    {
                        battleField[new Position { X = x, Y = y }] = Content.Wall;
                        recorder.AddEvent("View.Id", x, y, Direction.North, Content.Wall);
                    }
                    else
                    {
                        battleField[new Position { X = x, Y = y }] = Content.Empty;
                        recorder.AddEvent("View.Id", x, y, Direction.North, Content.Empty);
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

        private void MoveHead(View<Content> battleField, Fighter snake, Move newHeadPosition)
        {
            if (snake.Length != 0)
            {
                // replace old head with body
                battleField[new Position { X = snake.Head.Position.X, Y = snake.Head.Position.Y }] = Content.Body;
                recorder.AddEvent(snake.Id, snake.Head.Position.X, snake.Head.Position.Y, snake.Head.Direction, Content.Body);
            }
            // add head to fighter
            snake.Head = newHeadPosition;
            // put new head
            battleField[new Position { X = snake.Head.Position.X, Y = snake.Head.Position.Y }] = Content.Head;
            recorder.AddEvent(snake.Id, snake.Head.Position.X, snake.Head.Position.Y, snake.Head.Direction, Content.Head);
        }

        private void CutTail(View<Content> battleField, Fighter fighter)
        {
            // replace old tail with empty row
            battleField[new Position { X = fighter.Tail.Position.X, Y = fighter.Tail.Position.Y }] = Content.Empty;
            recorder.AddEvent("View.Id", fighter.Tail.Position.X, fighter.Tail.Position.Y, fighter.Tail.Direction, Content.Empty);

            // remove tail from fighter
            fighter.CutTail();

            // put new tail on field
            battleField[new Position { X = fighter.Tail.Position.X, Y = fighter.Tail.Position.Y }] = Content.Tail;
            recorder.AddEvent(fighter.Id, fighter.Tail.Position.X, fighter.Tail.Position.Y, fighter.Tail.Direction, Content.Tail);
        }

        private void PutWallsOnGateways(View<Content> battleField)
        {
            foreach (var gateway in gateways)
            {
                battleField[new Position { X = gateway.Position.X, Y = gateway.Position.Y }] = Content.Wall;
                recorder.AddEvent("View.Id", gateway.Position.X, gateway.Position.Y, gateway.Direction, Content.Gateway);
            }
        }
    }
}
