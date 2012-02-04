using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;
using Row = EatMySnake.Core.Battlefield.Row;

namespace EatMySnake.Core.Battlemanager
{
    public class Comparator
    {
        private readonly IBattleField _battleField;
        private ISnake _snake;

        public Comparator(IBattleField battleField, ISnake snake)
        {
            _snake = snake;
            _battleField = battleField;
        }

        public Move MakeDecision()
        {
            Move snakeHeadPositionOnBattleField = _snake.GetHeadPosition();

            foreach (var brainChip in _snake.BrainModules)
            {
                int chipSizeDim = brainChip.SizeX;

                switch (snakeHeadPositionOnBattleField.direction)
                {
                    case Direction.North:
                        {
                            GetView1(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView2(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView3(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.West:
                        {
                            GetView2(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView4(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView1(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.East:
                        {
                            GetView3(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView1(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView4(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.South:
                        {
                            GetView4(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView3(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            GetView2(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                }
            }

            /*
                 * hash map to store views

                    if(_chips.andType) all sums of sequences should be accepted

                    if(_chips.orType) one of all sums of sequences should be accepted
                        */
            throw new NotImplementedException();
        }

        private IEnumerable<Row> GetView1(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<Row>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(_battleField[x, y]);

            return rows;
        }

        private IEnumerable<Row> GetView2(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<Row>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx - cx; x < fx - cx + chipSizeDim; x++)
                    rows.Add(_battleField[y, x]);

            return rows;
        }

        private IEnumerable<Row> GetView3(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<Row>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy - cy; y < fy - cy + chipSizeDim; y++)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(_battleField[y, x]);

            return rows;
        }

        private IEnumerable<Row> GetView4(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim)
        {
            var rows = new List<Row>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionInBrainChip.X;
            int cy = snakeHeadPositionInBrainChip.Y;

            for (int y = fy + cy; y > fy + cy - chipSizeDim; y--)
                for (int x = fx + cx; x > fx + cx - chipSizeDim; x--)
                    rows.Add(_battleField[x, y]);

            return rows;
        }
    }
}