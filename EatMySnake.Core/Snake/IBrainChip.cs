using System.Collections.Generic;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Snake
{
    public interface IBrainChip
    {
        Move HeadPosition { get; set; }
        Size Size { get; }
        ChipRow this[int x, int y] { get; set; }
        IEnumerable<ChipRow> ToEnumeration();
    }
}