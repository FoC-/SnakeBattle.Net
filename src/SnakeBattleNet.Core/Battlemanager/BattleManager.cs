using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.ReplayRecorder;
using SnakeBattleNet.ReplayRecorder.Contracts;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class BattleManager
    {
        private Random random;
        private readonly IReplayRecorder recorder;

        public BattleManager(IReplayRecorder recorder)
        {
            this.recorder = recorder;
        }

        public void Fight(BattleField battleField, IList<Snake> snakes, int rounds)
        {
            if (snakes.Count > battleField.Gateways.Count)
                throw new Exception("Number of snakes is more then gateways");

            int randomSeed = Environment.TickCount;
            random = new Random(randomSeed);

            recorder.Initialize(battleField.SideLength, battleField.SideLength, randomSeed, snakes.Select(_ => _.Id).Concat(new[] { "battleField.Id" }));

            InitializeField(battleField, snakes);
            for (int i = 0; i < rounds; i++)
                Act(battleField, snakes);
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from the walls
        /// </summary>
        private void InitializeField(BattleField battleField, IList<Snake> snakes)
        {
            PutEmptySpaceAndWalls(battleField);

            //bite: gateways
            int n = 0;
            foreach (var snake in snakes)
                SnakeIsGrowing(battleField, snake, battleField.Gateways[n++]);

            //bite: 10 times empty space in front of head to grow
            foreach (var snake in snakes)
                for (int i = 0; i < 10; i++)
                    SnakeIsGrowing(battleField, snake, StraitMove(snake.Head));

            // remove tails
            foreach (var snake in snakes)
                CutTail(battleField, snake);

            PutWallsOnGateways(battleField);
        }

        /// <summary>
        /// Battle manager try to do one move for each snake
        /// </summary>
        private void Act(BattleField battleField, IEnumerable<Snake> snakes)
        {
            // use something like builder 
            foreach (var snake in snakes.Shuffle())
            {
                //Check if movement is possible
                var possibleMoves = new List<Move>(GetPossibleMoves(battleField, snake));
                if (possibleMoves.Count == 0) continue;

                //Try to move according brain chip
                Move move = GetLogicalMove(battleField, snake);
                if (possibleMoves.Contains(move))
                    TryToBite(battleField, snake, snakes, move);

                TryToBite(battleField, snake, snakes, possibleMoves[random.Next(possibleMoves.Count)]);
            }
        }

        /// <summary>
        /// We check rows around snake header trying to find passable
        /// </summary>
        /// <param name="battleField">Visible area around snake header</param>
        /// <param name="snake">Snake witch want to move</param>
        /// <returns>List of passable rows or current head position</returns>
        private IEnumerable<Move> GetPossibleMoves(BattleField battleField, Snake snake)
        {
            var possibleMoves = new List<Move>();

            int headX = snake.Head.Position.X;
            int headY = snake.Head.Position.Y;
            if (battleField[new Position { X = headX, Y = headY + 1 }] == Content.Empty || battleField[new Position { X = headX, Y = headY + 1 }] == Content.Tail)
                possibleMoves.Add(new Move(headX, headY + 1, Direction.North));
            if (battleField[new Position { X = headX, Y = headY - 1 }] == Content.Empty || battleField[new Position { X = headX, Y = headY - 1 }] == Content.Tail)
                possibleMoves.Add(new Move(headX, headY - 1, Direction.South));
            if (battleField[new Position { X = headX - 1, Y = headY }] == Content.Empty || battleField[new Position { X = headX - 1, Y = headY }] == Content.Tail)
                possibleMoves.Add(new Move(headX - 1, headY, Direction.West));
            if (battleField[new Position { X = headX + 1, Y = headY }] == Content.Empty || battleField[new Position { X = headX + 1, Y = headY }] == Content.Tail)
                possibleMoves.Add(new Move(headX + 1, headY, Direction.East));

            return possibleMoves;
        }

        /// <summary>
        /// Move in the current direction to the next row
        /// </summary>
        /// <param name="headPosition">Current head position</param>
        /// <returns>Next move in front of head</returns>
        private Move StraitMove(Move headPosition)
        {
            int headX = headPosition.Position.X;
            int headY = headPosition.Position.Y;
            var direction = headPosition.Direction;
            switch (direction)
            {
                case Direction.North:
                    return new Move(headX, headY + 1, Direction.North);
                case Direction.West:
                    return new Move(headX - 1, headY, Direction.West);
                case Direction.East:
                    return new Move(headX + 1, headY, Direction.East);
                case Direction.South:
                    return new Move(headX, headY - 1, Direction.South);
            }
            throw new Exception("Something wrong in moving strait!");
        }

        /// <summary>
        /// Make decision for next move
        /// </summary>
        /// <param name="battleField">Battle field or part of it</param>
        /// <param name="snake">Snake who try to move</param>
        /// <returns>Next move or null, if no idea where to move</returns>
        private Move GetLogicalMove(BattleField battleField, Snake snake)
        {
            var comparator = new Comparator(battleField, snake);
            return comparator.MakeDecision();
        }

        /// <summary>
        /// We try to bite any snake on field if new position of our head is equal to any tail position
        /// </summary>
        /// <param name="battleField"> </param>
        /// <param name="snakeBiter">Snake who try to bite</param>
        /// <param name="newHeadPosition">New position of header generated by initial algorithm</param>
        private void TryToBite(BattleField battleField, Snake snakeBiter, IEnumerable<Snake> snakes, Move newHeadPosition)
        {
            foreach (var snake in snakes)
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

        #region Draw on field
        private void PutEmptySpaceAndWalls(BattleField battleField)
        {
            for (int x = 0; x < battleField.SideLength; x++)
                for (int y = 0; y < battleField.SideLength; y++)
                    if (x == 0 || y == 0 || x == battleField.SideLength - 1 || y == battleField.SideLength - 1)
                    {
                        battleField[new Position { X = x, Y = y }] = Content.Wall;
                        recorder.AddEvent("battleField.Id", x, y, LookinkgTo(Direction.NoWay), Element.Wall);
                    }
                    else
                    {
                        battleField[new Position { X = x, Y = y }] = Content.Empty;
                        recorder.AddEvent("battleField.Id", x, y, LookinkgTo(Direction.NoWay), Element.Empty);
                    }
        }

        private void SnakeIsMoving(BattleField battleField, Snake snake, Move newHeadPosition)
        {
            CutTail(battleField, snake);
            MoveHead(battleField, snake, newHeadPosition);
        }

        private void SnakeIsbitting(BattleField battleField, Snake snakeBitter, Snake snakeBitten)
        {
            Move tailPosition = snakeBitten.Tail;
            CutTail(battleField, snakeBitten);
            MoveHead(battleField, snakeBitter, tailPosition);
        }

        private void SnakeIsGrowing(BattleField battleField, Snake snake, Move newHeadPosition)
        {
            MoveHead(battleField, snake, newHeadPosition);
        }

        private void MoveHead(BattleField battleField, Snake snake, Move newHeadPosition)
        {
            if (snake.Length != 0)
            {
                // replace old head with body
                battleField[new Position { X = snake.Head.Position.X, Y = snake.Head.Position.Y }] = Content.Body;
                recorder.AddEvent(snake.Id, snake.Head.Position.X, snake.Head.Position.Y, LookinkgTo(snake.Head.Direction), Element.Body);
            }
            // add head to snake
            snake.Head = newHeadPosition;
            // put new head
            battleField[new Position { X = snake.Head.Position.X, Y = snake.Head.Position.Y }] = Content.Head;
            recorder.AddEvent(snake.Id, snake.Head.Position.X, snake.Head.Position.Y, LookinkgTo(snake.Head.Direction), Element.Head);
        }

        private void CutTail(BattleField battleField, Snake snake)
        {
            // replace old tail with empty row
            battleField[new Position { X = snake.Tail.Position.X, Y = snake.Tail.Position.Y }] = Content.Empty;
            recorder.AddEvent("battleField.Id", snake.Tail.Position.X, snake.Tail.Position.Y, LookinkgTo(snake.Tail.Direction), Element.Empty);

            // remove tail from snake
            snake.CutTail();

            // put new tail on field
            battleField[new Position { X = snake.Tail.Position.X, Y = snake.Tail.Position.Y }] = Content.Tail;
            recorder.AddEvent(snake.Id, snake.Tail.Position.X, snake.Tail.Position.Y, LookinkgTo(snake.Tail.Direction), Element.Tail);
        }

        private void PutWallsOnGateways(BattleField battleField)
        {
            foreach (var gateway in battleField.Gateways)
            {
                battleField[new Position { X = gateway.Position.X, Y = gateway.Position.Y }] = Content.Wall;
                recorder.AddEvent("battleField.Id", gateway.Position.X, gateway.Position.Y, LookinkgTo(gateway.Direction), Element.Gateway);
            }
        }

        private Directed LookinkgTo(Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    return Directed.Down;
                case Direction.West:
                    return Directed.Left;
                case Direction.East:
                    return Directed.Right;
                case Direction.North:
                case Direction.NoWay:
                default:
                    return Directed.Up;
            }
        }
        #endregion Draw on field
    }
}
