using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlemanager
{
    public class Comparator
    {
        private readonly IBattleField battleField;
        private readonly ISnake snake;

        public Comparator(IBattleField battleField, ISnake snake)
        {
            this.snake = snake;
            this.battleField = battleField;
        }

        public Move MakeDecision()
        {
            Move snakeHeadPositionOnBattleField = this.snake.GetHeadPosition();

            foreach (var brainChip in this.snake.BrainModules)
            {
                int chipSizeDim = brainChip.Size.X;
                switch (snakeHeadPositionOnBattleField.direction)
                {
                    case Direction.North:
                        {
                            this.battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                        }
                        break;
                    case Direction.West:
                        {
                            this.battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                        }
                        break;
                    case Direction.East:
                        {
                            this.battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToNorth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                        }
                        break;
                    case Direction.South:
                        {
                            this.battleField.ViewToSouth(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToEast(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
                            this.battleField.ViewToWest(snakeHeadPositionOnBattleField, brainChip.GetOwnHead(), chipSizeDim);
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