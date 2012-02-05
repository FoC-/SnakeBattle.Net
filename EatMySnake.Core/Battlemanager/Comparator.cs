using System;
using System.Collections.Generic;
using EatMySnake.Core.Battlefield;
using EatMySnake.Core.Common;
using EatMySnake.Core.Snake;

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
                int chipSizeDim = brainChip.Size.X;
                switch (snakeHeadPositionOnBattleField.direction)
                {
                    case Direction.North:
                        {
                            _battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.West:
                        {
                            _battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.East:
                        {
                            _battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                    case Direction.South:
                        {
                            _battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                            _battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.HeadPosition, chipSizeDim);
                        }
                        break;
                }
            }

            /*
                    if(_chips.andType) all sums of sequences should be accepted

                    if(_chips.orType) one of all sums of sequences should be accepted
                        */
            throw new NotImplementedException();
        }
    }
}