using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public interface IBattleField
    {
        IList<Move> Gateways { get; }
        Size Size { get; }
        FieldRow this[int x, int y] { get; set; }
        FieldRow[] ViewToNorth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim);
        FieldRow[] ViewToWest(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim);
        FieldRow[] ViewToEast(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim);
        FieldRow[] ViewToSouth(Move snakeHeadPositionOnBattleField, Move snakeHeadPositionOnModule, int chipSizeDim);
    }
}