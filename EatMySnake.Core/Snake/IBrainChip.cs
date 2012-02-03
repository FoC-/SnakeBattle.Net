using EatMySnake.Core.Common;

namespace EatMySnake.Core.Snake
{
    public interface IBrainChip {
        Move HeadPosition { get; set; }
        int SizeX { get; }
        int SizeY { get; }
        Row this[int x, int y] { get; set; }
    }
}