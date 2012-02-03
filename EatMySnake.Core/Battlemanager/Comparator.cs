using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlefield.Implementation;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battlemanager
{
    public class Comparator
    {
        IBattleField _battleField = new BattleField();
        private Matrix GetViewPort(Snake.Implementation.Snake snake)
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

        private void DetermDirection(Snake.Implementation.Snake snake, Matrix observableArea)
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