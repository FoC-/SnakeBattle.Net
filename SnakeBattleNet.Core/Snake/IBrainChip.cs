using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake
{
    public interface IBrainChip
    {
        Move HeadPosition { get; set; }
        Size Size { get; }
        ChipRow this[int x, int y] { get; set; }
        IEnumerable<ChipRow> ToEnumeration();
    }
}