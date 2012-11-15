using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;
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

        public void Fight(IBattleField battleField, IList<ISnake> snakes, int rounds)
        {
            if (snakes.Count > battleField.Gateways.Count)
                throw new Exception("Number of snakes is more then gateways");

            int randomSeed = Environment.TickCount;
            random = new Random(randomSeed);

            recorder.Initialize(battleField.Size.X, battleField.Size.X, randomSeed, snakes.Select(_ => _.Id).Concat(new[] { battleField.Id }));

            InitializeField(battleField, snakes);
            for (int i = 0; i < rounds; i++)
                Act(battleField, snakes);
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from the walls
        /// </summary>
        private void InitializeField(IBattleField battleField, IList<ISnake> snakes)
        {
            //bite: gateways
            int n = 0;
            foreach (var snake in snakes)
                SnakeIsGrowing(battleField, snake, battleField.Gateways[n++]);

            //bite: 10 times empty space in front of head to grow
            foreach (var snake in snakes)
                for (int i = 0; i < 10; i++)
                    SnakeIsGrowing(battleField, snake, StraitMove(snake.GetHeadPosition()));

            // remove tails
            foreach (var snake in snakes)
                CutTail(battleField, snake);

            PutWallsOnGateways(battleField);
        }

        /// <summary>
        /// Battle manager try to do one move for each snake
        /// </summary>
        private void Act(IBattleField battleField, IEnumerable<ISnake> snakes)
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
        private IEnumerable<Move> GetPossibleMoves(IBattleField battleField, ISnake snake)
        {
            var possibleMoves = new List<Move>();

            int headX = snake.GetHeadPosition().X;
            int headY = snake.GetHeadPosition().Y;

            if (battleField[headX, headY + 1].FieldRowContent == FieldRowContent.Empty || battleField[headX, headY + 1].FieldRowContent == FieldRowContent.Tail)
                possibleMoves.Add(new Move(headX, headY + 1, Direction.North));
            if (battleField[headX, headY - 1].FieldRowContent == FieldRowContent.Empty || battleField[headX, headY - 1].FieldRowContent == FieldRowContent.Tail)
                possibleMoves.Add(new Move(headX, headY - 1, Direction.South));
            if (battleField[headX - 1, headY].FieldRowContent == FieldRowContent.Empty || battleField[headX - 1, headY].FieldRowContent == FieldRowContent.Tail)
                possibleMoves.Add(new Move(headX - 1, headY, Direction.West));
            if (battleField[headX + 1, headY].FieldRowContent == FieldRowContent.Empty || battleField[headX + 1, headY].FieldRowContent == FieldRowContent.Tail)
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
            int headX = headPosition.X;
            int headY = headPosition.Y;
            var direction = headPosition.direction;
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
        private Move GetLogicalMove(IBattleField battleField, ISnake snake)
        {
            var comparator = new Comparator(battleField, snake);
            return comparator.MakeDecision();
        }

        /// <summary>
        /// We try to bite any snake on field if new position of our head is equal to any tail position
        /// </summary>
        /// <param name="snakeBiter">Snake who try to bite</param>
        /// <param name="newHeadPosition">New position of header generated by initial algorithm</param>
        private void TryToBite(IBattleField battleField, ISnake snakeBiter, IEnumerable<ISnake> snakes, Move newHeadPosition)
        {
            foreach (var snake in snakes)
            {
                if (snakeBiter.GetHeadPosition().Equals(snake.GetTailPosition()))
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
        private void SnakeIsMoving(IBattleField battleField, ISnake snake, Move newHeadPosition)
        {
            CutTail(battleField, snake);
            MoveHead(battleField, snake, newHeadPosition);
        }

        private void SnakeIsbitting(IBattleField battleField, ISnake snakeBitter, ISnake snakeBitten)
        {
            Move tailPosition = snakeBitten.GetTailPosition();
            CutTail(battleField, snakeBitten);
            MoveHead(battleField, snakeBitter, tailPosition);
        }

        private void SnakeIsGrowing(IBattleField battleField, ISnake snake, Move newHeadPosition)
        {
            MoveHead(battleField, snake, newHeadPosition);
        }

        private void MoveHead(IBattleField battleField, ISnake snake, Move newHeadPosition)
        {
            if (snake.Length != 0)
            {
                // replace old head with body
                battleField[snake.GetHeadPosition().X, snake.GetHeadPosition().Y] = new FieldRow(FieldRowContent.Body, snake.Id);
                recorder.AddEvent(snake.Id, snake.GetHeadPosition().X, snake.GetHeadPosition().Y, LookinkgTo(snake.GetHeadPosition().direction), Element.Body);
            }
            // add head to snake
            snake.SetHead(newHeadPosition);
            // put new head
            battleField[snake.GetHeadPosition().X, snake.GetHeadPosition().Y] = new FieldRow(FieldRowContent.Head, snake.Id);
            recorder.AddEvent(snake.Id, snake.GetHeadPosition().X, snake.GetHeadPosition().Y, LookinkgTo(snake.GetHeadPosition().direction), Element.Head);
        }

        private void CutTail(IBattleField battleField, ISnake snake)
        {
            // replace old tail with empty row
            battleField[snake.GetTailPosition().X, snake.GetTailPosition().Y] = new FieldRow(FieldRowContent.Empty, battleField.Id);
            recorder.AddEvent(battleField.Id, snake.GetTailPosition().X, snake.GetTailPosition().Y, LookinkgTo(snake.GetTailPosition().direction), Element.Empty);

            // remove tail from snake
            snake.RemoveTail();

            // put new tail on field
            battleField[snake.GetTailPosition().X, snake.GetTailPosition().Y] = new FieldRow(FieldRowContent.Tail, snake.Id);
            recorder.AddEvent(snake.Id, snake.GetTailPosition().X, snake.GetTailPosition().Y, LookinkgTo(snake.GetTailPosition().direction), Element.Tail);
        }

        private void PutWallsOnGateways(IBattleField battleField)
        {
            foreach (var gateway in battleField.Gateways)
            {
                battleField[gateway.X, gateway.Y] = new FieldRow(FieldRowContent.Wall, battleField.Id);
                recorder.AddEvent(battleField.Id, gateway.X, gateway.Y, LookinkgTo(gateway.direction), Element.Gateway);
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
