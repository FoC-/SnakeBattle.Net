using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;
using EatMySnake.Core.Extensions;

namespace EatMySnake.Core.Battle
{
    public class BattleManager
    {
        Random random = new Random();
        private IBattleField _battleField;
        private List<ISnake> _snakes;

        public BattleManager(IBattleField battleField, List<ISnake> snakes)
        {
            if (snakes.Count > battleField.Gateways.Count)
                throw new Exception("Number of snakes is more then gateways");

            _battleField = battleField;
            _snakes = snakes;
            SetupHandlers();
            InitializeField();
        }

        public void Act()
        {
            foreach (Snake snake in _snakes.Shuffle())
            {
                //todo: generate visible area for snake and put it to next move for analyze
                Move newHeadPosition = NextMove(_battleField, snake);
                TryToBite(snake, newHeadPosition);
            }
        }

        private void SetupHandlers()
        {
            foreach (var snake in _snakes)
            {
                snake.Moving += SnakeMoving;
                snake.Biting += SnakeBiting;
                snake.Dead += SnakeDead;
            }
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from the middle of the wall
        /// </summary>
        private void InitializeField()
        {
            //bite: gateways
            int n = 0;
            foreach (Snake snake in _snakes)
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
                _battleField[gateway.X, gateway.Y] = new Row(Content.Wall);
        }

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

        private void SnakeMoving(object sender, EventArgs e)
        {
            Snake snake = sender as Snake;
            Move move = e as Move;
            if (snake.Length == 0) return;
            Move head = snake.GetHeadPosition();
            Move tail = snake.GetTailPosition();
            _battleField[head.X, head.Y].Content = Content.OwnBody;
            _battleField[move.X, move.Y].Content = Content.OwnHead;
            _battleField[tail.X, tail.Y].Content = Content.Empty;
            Console.WriteLine(snake.Name + " Move " + move + " Len = " + snake.Length);
        }

        private void SnakeBiting(object sender, EventArgs e)
        {
            Snake snake = sender as Snake;
            Move move = e as Move;
            if (snake.Length == 0) return;
            Move head = snake.GetHeadPosition();
            _battleField[head.X, head.Y].Content = Content.OwnBody;
            _battleField[move.X, move.Y].Content = Content.OwnHead;
            Console.WriteLine(snake.Name + " Bite " + move + " Len = " + snake.Length);
        }

        private void SnakeDead(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private Move NextMove(IBattleField viewPort, Snake snake)
        {
            //Check if movement is possible
            List<Move> possibleMoves = CheckPossibleMoves(viewPort, snake);
            //check is first move is not head position
            if (possibleMoves[0].Equals(snake.GetHeadPosition()))
            {
                //Return current position of head
                return snake.GetHeadPosition();
            }
            //Try to move according brainchip
            //foreach (Matrix brainModule in snake.BrainModules)
            //{
            //    throw new NotImplementedException();
            //}
            //Move in random direction
            return possibleMoves[random.Next(possibleMoves.Count)];
        }

        /// <summary>
        /// We try to bite any snake on field if new position of our head is equal to any tail position
        /// </summary>
        /// <param name="snakeBiter">Snake who try to bite</param>
        /// <param name="newHeadPosition">New position of header genarated by initial algorithm</param>
        private void TryToBite(Snake snakeBiter, Move newHeadPosition)
        {
            foreach (Snake snake in _snakes)
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

        /// <summary>
        /// We check rows around snake header trying to find passable
        /// </summary>
        /// <param name="viewPort">Vissible area around snake header</param>
        /// <param name="snake">Snake witch want to move</param>
        /// <returns>List of passable rows or current head position</returns>
        private List<Move> CheckPossibleMoves(IBattleField viewPort, Snake snake)
        {
            List<Move> possibleMoves = new List<Move>();

            List<Content> passable = new List<Content>();
            passable.Add(Content.Empty);
            passable.Add(Content.EnemyTail);
            passable.Add(Content.OwnTail);

            int headX = snake.GetHeadPosition().X;
            int headY = snake.GetHeadPosition().Y;

            foreach (Content content in passable)
            {
                if (viewPort[headX, headY + 1].Content == content) possibleMoves.Add(new Move(headX, headY + 1, Direction.North));
                if (viewPort[headX, headY - 1].Content == content) possibleMoves.Add(new Move(headX, headY - 1, Direction.South));
                if (viewPort[headX - 1, headY].Content == content) possibleMoves.Add(new Move(headX - 1, headY, Direction.West));
                if (viewPort[headX + 1, headY].Content == content) possibleMoves.Add(new Move(headX + 1, headY, Direction.East));
            }
            if (0 == possibleMoves.Count) possibleMoves.Add(snake.GetHeadPosition());
            return possibleMoves;
        }

        private Matrix GetViewPort(Snake snake)
        {
            //todo: need to check if head near borders
            Move headPosition = snake.GetHeadPosition();

            int maxX, minX;
            if (headPosition.X + snake.VisionRadius > _battleField.SizeX)
            {
                maxX = _battleField.SizeX - headPosition.X;
            }
            else
            {
                maxX = headPosition.X + snake.VisionRadius;
            }

            if (headPosition.X - snake.VisionRadius < 0)
            {
                minX = headPosition.X;
            }
            else
            {
                minX = headPosition.X - snake.VisionRadius;
            }

            int maxY, minY;
            if (headPosition.Y + snake.VisionRadius > _battleField.SizeY)
            {
                maxY = _battleField.SizeY - headPosition.Y;
            }
            else
            {
                maxY = headPosition.Y + snake.VisionRadius;
            }

            if (headPosition.Y - snake.VisionRadius < 0)
            {
                minY = headPosition.Y;
            }
            else
            {
                minY = headPosition.Y - snake.VisionRadius;
            }

            //todo: error exist need to verify logic
            Matrix tmpArea = new Matrix(maxX, maxY);
            int tx = 0, ty = 0;
            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    tmpArea[tx, ty] = _battleField[x, y];
                    ty++;
                }
                tx++;
            }
            return tmpArea;
        }

        private void DetermDirection(Snake snake, Matrix observableArea)
        {
            List<Matrix> tmpObservAreas = CreateRotatedMatrix(observableArea);
            foreach (Matrix brainModule in snake.BrainModules)
            {

            }
            throw new NotImplementedException();
        }

        private List<Matrix> CreateRotatedMatrix(Matrix area)
        {
            throw new NotImplementedException();
        }
    }
}
