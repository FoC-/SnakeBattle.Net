using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public interface IBattleField
    {
        IList<Move> Gateways { get; }
        Size Size { get; }
        FieldRow this[int x, int y] { get; set; }
        FieldRow[] ViewToNorth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnBrainChip, int chipSizeDim);
        FieldRow[] ViewToWest(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnBrainChip, int chipSizeDim);
        FieldRow[] ViewToEast(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnBrainChip, int chipSizeDim);
        FieldRow[] ViewToSouth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnBrainChip, int chipSizeDim);
    }
}