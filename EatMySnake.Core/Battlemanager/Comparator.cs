using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Battlefield.Implementation;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;
using Row = EatMySnake.Core.Battlefield.Row;

namespace EatMySnake.Core.Battlemanager
{
    public class Comparator
    {
        private readonly IBattleField _battleField;
        private readonly IList<IBrainChip> _chips;
        private ISnake _snake;

        public Comparator(IBattleField battleField, ISnake snake, IList<IBrainChip> chips)
        {
            _snake = snake;
            _battleField = battleField;
            _chips = chips;
        }

        public Move MakeDecision()
        {

            foreach (var brainChip in _chips)
            {
                int cx = brainChip.HeadPosition.X;
                int cy = brainChip.HeadPosition.Y;

                int fx = _snake.GetHeadPosition().X;
                int fy = _snake.GetHeadPosition().Y;

                int chipSizeDim = brainChip.SizeX;

                List<Row> rows = new List<Row>();

                //view 1+
                for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                    for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                        rows.Add(_battleField[x, y]);

                //view 2+
                for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                    for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                        rows.Add(_battleField[y, x]);

                //view 3+
                for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                    for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                        rows.Add(_battleField[y, x]);

                //view 4+
                for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                    for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                        rows.Add(_battleField[x, y]);

                /*
                 * hash map to store views
                    get ranges for current head positions

                    get camera view pov

                    if(_chips.andType) all sums of sequences should be accepted

                    if(_chips.orType) one of all sums of sequences should be accepted
                        */

            }
            throw new NotImplementedException();
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