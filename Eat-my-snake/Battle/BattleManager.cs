using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class BattleManager
    {
        Random random = new Random();
        private BattleField _battleField;
        private List<Snake> _snakes;

        public BattleManager()
        {
            _battleField = new BattleField();
            _snakes = new List<Snake>();
        }

        public void Move()
        {
            //we need shuffle snakes order for moves
            ShuffleSnakes();
            Move newHeadPosition;
            foreach (Snake snake in _snakes)
            {
                //todo: genarate vissible area for snake and put it to next move for analyze
                newHeadPosition = NextMove(_battleField.CurrentState, snake);
                TryToBite(snake, newHeadPosition);
            }

            //_battlefield should been updated
        }

        private Move NextMove(Matrix viewPort, Snake snake)
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
            foreach (Matrix brainModule in snake.BrainModules)
            {
                throw new NotImplementedException();
            }
            //Move in random direction
            return possibleMoves[random.Next(possibleMoves.Count)];
        }

        /// <summary>
        /// We try to bite any snake on field if new position of our head is equal to any tail position
        /// </summary>
        /// <param name="snakeBiter">Snake who try to bite</param>
        /// <param name="newHeadPosition">New position of header genarated by initial algorithm</param>
        private void TryToBite(Snake snakeBiter,Move newHeadPosition)
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
        }

        /// <summary>
        /// We check rows around snake header trying to find passable
        /// </summary>
        /// <param name="viewPort">Vissible area around snake header</param>
        /// <param name="snake">Snake witch want to move</param>
        /// <returns>List of passable rows or current head position</returns>
        private List<Move> CheckPossibleMoves(Matrix viewPort, Snake snake)
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

        private Matrix GetObservableArea(Snake snake)
        {
            //todo: need to check if head near borders
            Move headPosition = snake.GetHeadPosition();

            int maxX = 0, minX = 0;
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

            int maxY = 0, minY = 0;
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
                    tmpArea[tx, ty] = _battleField.CurrentState[x, y];
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
        }

        private List<Matrix> CreateRotatedMatrix(Matrix area)
        {
            throw new NotImplementedException();
        }
    }
}
