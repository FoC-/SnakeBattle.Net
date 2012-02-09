using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battlefield
{
    public interface IBattleField
    {
        IList<Move> Gateways { get; }
        Size Size { get; }
        FieldRow this[int x, int y] { get; set; }
        IEnumerable<FieldRow> ViewToNorth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim);
        IEnumerable<FieldRow> ViewToWest(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim);
        IEnumerable<FieldRow> ViewToEast(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim);
        IEnumerable<FieldRow> ViewToSouth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionInBrainChip, int chipSizeDim);
    }
}