using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battlefield
{
    public interface IBattleField
    {
        IList<Move> Gateways { get; }
        int SizeX { get; }
        int SizeY { get; }
        Row this[int x, int y] { get; set; }
    }
}