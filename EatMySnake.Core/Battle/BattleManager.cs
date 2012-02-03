using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

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
            InitializeField();
        }

        /// <summary>
        /// Initialize battle field with snakes, each snake "growth" from th middle of the wall
        /// </summary>
        public void InitializeField()
        {
            //set heads positions (bite: walls in the mmiddle) 
            int n = 0;
            foreach (Snake snake in _snakes)
            {
                snake.Biting += snake_Biting;
                snake.Moving += snake_Moving;
                Move move = _battleField.Gateways[n++];
                snake.Bite(move);
                //draw snake heads
                _battleField[move.X, move.Y] = new Row(Content.OwnHead);
            }
            //move each head in direction (bite: 9 times empty space in front of head)
            for (int i = 0; i < 9; i++)
            {
                foreach (Snake snake in _snakes)
                {
                    int headX = snake.GetHeadPosition().X;
                    int headY = snake.GetHeadPosition().Y;
                    Direction direction = snake.GetHeadPosition().direction;
                    switch (direction)
                    {
                        case Direction.North:
                            snake.Bite(new Move(headX, headY + 1, Direction.North));
                            break;
                        case Direction.West:
                            snake.Bite(new Move(headX - 1, headY, Direction.West));
                            break;
                        case Direction.East:
                            snake.Bite(new Move(headX + 1, headY, Direction.East));
                            break;
                        case Direction.South:
                            snake.Bite(new Move(headX, headY - 1, Direction.South));
                            break;
                    }
                }
            }
            //move one more time (move: once to empty walls from tails)
            foreach (Snake snake in _snakes)
            {
                int headX = snake.GetHeadPosition().X;
                int headY = snake.GetHeadPosition().Y;
                Direction direction = snake.GetHeadPosition().direction;
                switch (direction)
                {
                    case Direction.North:
                        snake.NextMove(new Move(headX, headY + 1, Direction.North));
                        break;
                    case Direction.West:
                        snake.NextMove(new Move(headX - 1, headY, Direction.West));
                        break;
                    case Direction.East:
                        snake.NextMove(new Move(headX + 1, headY, Direction.East));
                        break;
                    case Direction.South:
                        snake.NextMove(new Move(headX, headY - 1, Direction.South));
                        break;
                }
            }
            foreach (var gateway in _battleField.Gateways)
            {
                _battleField[gateway.X, gateway.Y] = new Row(Content.Wall);
            }
        }

        void snake_Moving(object o, EventArgs e)
        {
            Snake snake = o as Snake;
            Move move = e as Move;
            if (snake.Length == 0) return;
            Move head = snake.GetHeadPosition();
            Move tail = snake.GetTailPosition();
            _battleField[head.X, head.Y].Content = Content.OwnBody;
            _battleField[move.X, move.Y].Content = Content.OwnHead;
            _battleField[tail.X, tail.Y].Content = Content.Empty;
            Console.WriteLine(snake.Name + " Move " + move + " Len = " + snake.Length);
        }

        void snake_Biting(object o, EventArgs e)
        {
            Snake snake = o as Snake;
            Move move = e as Move;
            if (snake.Length == 0) return;
            Move head = snake.GetHeadPosition();
            _battleField[head.X, head.Y].Content = Content.OwnBody;
            _battleField[move.X, move.Y].Content = Content.OwnHead;
            Console.WriteLine(snake.Name + " Bite " + move + " Len = " + snake.Length);
        }

        public void Move()
        {
            Move newHeadPosition;
            //we need shuffle snakes order for moves
            ShuffleSnakes();
            //each snake should make own move
            foreach (Snake snake in _snakes)
            {
                //todo: genarate vissible area for snake and put it to next move for analyze
                newHeadPosition = NextMove(_battleField, snake);
                TryToBite(snake, newHeadPosition);
            }

            //_battlefield should been updated
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

        //todo restuta->foc: I've already wrote shuffle, you should review changes made by me
        /// <summary>
        /// Shuffle snakes to genarate new sequense in witch snakes will move
        /// </summary>
        private void ShuffleSnakes()
        {
            int n = _snakes.Count;
            int p;
            while (n > 0)
            {
                p = random.Next(n);
                _snakes.Add(_snakes[p]);
                _snakes.RemoveAt(p);
                n--;
            }
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

        public void DetermDirection(Snake snake, Matrix observableArea)
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
