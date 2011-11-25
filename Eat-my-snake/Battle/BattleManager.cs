using System;
using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battle
{
    public class BattleManager
    {
        private BattleField _battleField;
        private List<Snake> _snakes;

        public BattleManager()
        {
            _battleField = new BattleField();
            _snakes = new List<Snake>();
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

        private Matrix GetObservableArea(Snake snake)
        {
            //todo: need to check if head near borders
            Move headPosition = snake.GetHeadPosition();

            int maxX = 0, minX = 0;
            if (headPosition.x + snake.VisionRadius > _battleField.SizeX)
            {
                maxX = _battleField.SizeX - headPosition.x;
            }
            else
            {
                maxX = headPosition.x + snake.VisionRadius;
            }

            if (headPosition.x - snake.VisionRadius < 0)
            {
                minX = headPosition.x;
            }
            else
            {
                minX = headPosition.x - snake.VisionRadius;
            }

            int maxY = 0, minY = 0;
            if (headPosition.y + snake.VisionRadius > _battleField.SizeY)
            {
                maxY = _battleField.SizeY - headPosition.y;
            }
            else
            {
                maxY = headPosition.y + snake.VisionRadius;
            }

            if (headPosition.y - snake.VisionRadius < 0)
            {
                minY = headPosition.y;
            }
            else
            {
                minY = headPosition.y - snake.VisionRadius;
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

        public Move NextMove(Matrix currentStateOfBattleField)
        {
            //Check if movement is possible
            //if (CheckPossibleMoves(currentStateOfBattleField)[0] == Direction.NoWay)
            //{
            //    //Return current position of head
            //    return new Move();
            //}

            Snake snake = new Snake();

            throw new NotImplementedException();
            //After calculation we should return new positon of head
            //add leangth if any tail is eaten or add new head and delete tail);
        }

        //private List<Direction> CheckPossibleMoves(Matrix currentStateOfBattleField)
        //{
        //    List<Direction> directions = new List<Direction>();
        //    int headX = BodyParts[0].x;
        //    int headY = BodyParts[0].y;
        //    if (currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.OwnTail)
        //        directions.Add(Direction.North);
        //    if (currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.South);
        //    if (currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.West);
        //    if (currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.East);
        //    if (directions.Count == 0)
        //    {
        //        directions.Add(Direction.NoWay);
        //    }
        //    return directions;
        //}
    }
}
