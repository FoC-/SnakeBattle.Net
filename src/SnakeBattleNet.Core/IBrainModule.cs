using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core
{
    public interface IBrainModule
    {
        Size Size { get; }
        AOColor HeadColor { get; }

        ModuleRow[,] ModuleRows { get; }
        ModuleRow this[int x, int y] { get; }
        ModuleRow[] ToArray();

        void SetWall(int x, int y, Exclude exclude, AOColor aoColor);
        void SetEmpty(int x, int y, Exclude exclude, AOColor aoColor);
        void SetUndefinied(int x, int y);

        void SetEnemyHead(int x, int y, Exclude exclude, AOColor aoColor);
        void SetEnemyBody(int x, int y, Exclude exclude, AOColor aoColor);
        void SetEnemyTail(int x, int y, Exclude exclude, AOColor aoColor);

        Move GetOwnHead();
        void SetOwnHead(int x, int y, AOColor aoColor, Direction direction);
        void SetOwnBody(int x, int y, Exclude exclude, AOColor aoColor);
        void SetOwnTail(int x, int y, Exclude exclude, AOColor aoColor);
    }
}