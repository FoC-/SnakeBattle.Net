using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class BattleManager
    {
        private Random random;
        private readonly IBattleField battleField;
        private readonly IList<ISnake> snakes;
        private readonly IReplayRecorder recorder;

        public BattleManager(IBattleField battleField, IList<ISnake> snakes, IReplayRecorder recorder)
        {
            if (snakes.Count > battleField.Gateways.Count)
                throw new Exception("Number of snakes is more then gateways");

            this.battleField = battleField;
            this.snakes = snakes;
            this.recorder = recorder;

            InitalizeRandom();
            recorder.InitSnakes(snakes);
            recorder.InitBattleField(battleField);
        }

        private void InitalizeRandom()
        {
            int randomSeed = Environment.TickCount;

            this.random = new Random(randomSeed);
            recorder.InitSeed(randomSeed);
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from the walls
        /// </summary>
        public void InitializeField()
        {
            //bite: gateways
            int n = 0;
            foreach (var snake in snakes)
                SnakeIsGrowing(snake, battleField.Gateways[n++]);

            //bite: 10 times empty space in front of head to grow
            foreach (var snake in snakes)
                for (int i = 0; i < 10; i++)
                    SnakeIsGrowing(snake, StraitMove(snake.GetHeadPosition()));

            // remove tails
            foreach (var snake in snakes)
                CutTail(snake);

            PutWallsOnGateways();
        }

        /// <summary>
        /// Battle manager try to do one move for each snake
        /// </summary>
        public void Act()
        {
            // use something like builder 
            //StartGatherEvents();
            foreach (var snake in snakes.Shuffle())
            {
                //Check if movement is possible
                var possibleMoves = new List<Move>(GetPossibleMoves(battleField, snake));
                if (possibleMoves.Count == 0) continue;

                //Try to move according brain chip
                Move move = GetLogicalMove(battleField, snake);
                if (possibleMoves.Contains(move))
                    TryToBite(snake, move);

                TryToBite(snake, possibleMoves[random.Next(possibleMoves.Count)]);
            }
            //StopGatherEvents();
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
        private void TryToBite(ISnake snakeBiter, Move newHeadPosition)
        {
            foreach (var snake in snakes)
            {
                if (snakeBiter.GetHeadPosition().Equals(snake.GetTailPosition()))
                {
                    if (snakeBiter.Id == snake.Id)
                        SnakeIsMoving(snakeBiter, newHeadPosition);
                    else
                        SnakeIsbitting(snakeBiter, snake);
                    return;
                }
            }
            SnakeIsMoving(snakeBiter, newHeadPosition);
        }

        #region Draw on field
        private void SnakeIsMoving(ISnake snake, Move newHeadPosition)
        {
            CutTail(snake);
            MoveHead(snake, newHeadPosition);
        }

        private void SnakeIsbitting(ISnake snakeBitter, ISnake snakeBitten)
        {
            Move tMove = snakeBitten.GetTailPosition();
            CutTail(snakeBitten);
            MoveHead(snakeBitter, tMove);
        }

        private void SnakeIsGrowing(ISnake snake, Move newHeadPosition)
        {
            MoveHead(snake, newHeadPosition);
        }

        private void MoveHead(ISnake snake, Move newHeadPosition)
        {
            if (snake.Length != 0)
            {
                // replace old head with body
                battleField[snake.GetHeadPosition().X, snake.GetHeadPosition().Y] = new FieldRow(FieldRowContent.Body, snake.Id);
                recorder.AddEvent(snake.Id, snake.GetHeadPosition().X, snake.GetHeadPosition().Y, Command.PutBody);
            }
            // add head to snake
            snake.SetHead(newHeadPosition);
            // put new head
            battleField[snake.GetHeadPosition().X, snake.GetHeadPosition().Y] = new FieldRow(FieldRowContent.Head, snake.Id);
            recorder.AddEvent(snake.Id, snake.GetHeadPosition().X, snake.GetHeadPosition().Y, Command.PutHead);
        }

        private void CutTail(ISnake snake)
        {
            // replace old tail with empty row
            battleField[snake.GetTailPosition().X, snake.GetTailPosition().Y] = new FieldRow(FieldRowContent.Empty);
            recorder.AddEvent(null, snake.GetTailPosition().X, snake.GetTailPosition().Y, Command.PutEmpty);

            // remove tail from snake
            snake.RemoveTail();

            // put new tail on field
            battleField[snake.GetTailPosition().X, snake.GetTailPosition().Y] = new FieldRow(FieldRowContent.Tail, snake.Id);
            recorder.AddEvent(snake.Id, snake.GetTailPosition().X, snake.GetTailPosition().Y, Command.PutTail);
        }

        private void PutWallsOnGateways()
        {
            foreach (var gateway in battleField.Gateways)
            {
                battleField[gateway.X, gateway.Y] = new FieldRow(FieldRowContent.Wall);
                recorder.AddEvent(null, gateway.X, gateway.Y, Command.PutGateway);
            }
        }
        #endregion Draw on field
    }
}
