using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;
using SnakeBattleNet.Utils.Extensions;

namespace EatMySnake.Core.Battlemanager
{
    public class BattleManager
    {
        readonly Random _random = new Random();
        private readonly IBattleField _battleField;
        private readonly IList<ISnake> _snakes;

        public BattleManager(IBattleField battleField, IList<ISnake> snakes)
        {
            if (snakes.Count > battleField.Gateways.Count)
                throw new Exception("Number of snakes is more then gateways");

            _battleField = battleField;
            _snakes = snakes;
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from the walls
        /// </summary>
        public void InitializeField()
        {
            //bite: gateways
            int n = 0;
            foreach (var snake in _snakes)
                snake.Bite(_battleField.Gateways[n++]);

            //bite: 10 times empty space in front of head to grow
            foreach (var snake in _snakes)
                for (int i = 0; i < 10; i++)
                    snake.Bite(StraitMove(snake.GetHeadPosition()));

            //bitten: once to empty walls from tails
            foreach (var snake in _snakes)
                snake.Bitten();

            //draw walls
            foreach (var gateway in _battleField.Gateways)
                _battleField[gateway.X, gateway.Y] = new FieldRow(Content.Wall);
        }

        /// <summary>
        /// Battle manager try to do one move for each snake
        /// </summary>
        public void Act()
        {
            foreach (var snake in _snakes.Shuffle())
            {
                //Check if movement is possible
                var possibleMoves = new List<Move>(GetPossibleMoves(_battleField, snake));
                if (possibleMoves.Count == 0) continue;

                //Try to move according brain chip
                Move move = GetLogicalMove(_battleField, snake);
                TryToBite(snake, move ?? possibleMoves[_random.Next(possibleMoves.Count)]);
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

            if (battleField[headX, headY + 1].Content == Content.Empty || battleField[headX, headY + 1].Content == Content.Tail)
                possibleMoves.Add(new Move(headX, headY + 1, Direction.North));
            if (battleField[headX, headY - 1].Content == Content.Empty || battleField[headX, headY - 1].Content == Content.Tail)
                possibleMoves.Add(new Move(headX, headY - 1, Direction.South));
            if (battleField[headX - 1, headY].Content == Content.Empty || battleField[headX - 1, headY].Content == Content.Tail)
                possibleMoves.Add(new Move(headX - 1, headY, Direction.West));
            if (battleField[headX + 1, headY].Content == Content.Empty || battleField[headX + 1, headY].Content == Content.Tail)
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
#warning Kush: need to finish this
            return null;
        }

        /// <summary>
        /// We try to bite any snake on field if new position of our head is equal to any tail position
        /// </summary>
        /// <param name="snakeBiter">Snake who try to bite</param>
        /// <param name="newHeadPosition">New position of header generated by initial algorithm</param>
        private void TryToBite(ISnake snakeBiter, Move newHeadPosition)
        {
            foreach (var snake in _snakes)
            {
                if (snakeBiter.GetHeadPosition().Equals(snake.GetTailPosition()))
                {
                    snakeBiter.Bite(newHeadPosition);
                    snake.Bitten();
                    return;
                }
            }
            snakeBiter.NextMove(newHeadPosition);
        }

        #region Events for displaying on battlefield
        public void SetupHandlers()
        {
            foreach (var snake in _snakes)
            {
                snake.Moving += SnakeMoving;
                snake.Biting += SnakeBiting;
                snake.Dead += SnakeDead;
            }
        }

        private void SnakeMoving(object sender, EventArgs e)
        {
            var snake = sender as ISnake;
            var newHead = e as Move;
            _battleField[newHead.X, newHead.Y] = new FieldRow(Content.Head, snake.Guid);
            Console.WriteLine("{0} Move {1} Len = {2}", snake.Name, newHead, snake.Length);

            var oldHead = snake.GetHeadPosition();
            if (oldHead == null) return;
            _battleField[oldHead.X, oldHead.Y] = new FieldRow(Content.Body, snake.Guid);

#warning New tail is not appear on the battlefield
            var oldTail = snake.GetTailPosition();
            _battleField[oldTail.X, oldTail.Y] = new FieldRow();
        }

        private void SnakeBiting(object sender, EventArgs e)
        {
            var snake = sender as ISnake;
            var newHead = e as Move;
            _battleField[newHead.X, newHead.Y] = new FieldRow(Content.Head, snake.Guid);
            Console.WriteLine("{0} Bite {1} Len = {2}", snake.Name, newHead, snake.Length);

            var oldHead = snake.GetHeadPosition();
            if (oldHead == null) return;
            _battleField[oldHead.X, oldHead.Y] = new FieldRow(Content.Body, snake.Guid);
        }

        private void SnakeDead(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
