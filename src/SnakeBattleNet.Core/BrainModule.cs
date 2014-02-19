using System.Collections.Generic;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core
{
    public class BrainModule
    {
        private readonly string _snakeId;
        private Move _ownHead;

        public BrainModule(Size size, string snakeId)
        {
            Size = size;
            _snakeId = snakeId;
            ModuleRows = new ModuleRow[Size.X, Size.Y];
            HeadColor = AOColor.AndGrey;
            InitilaizeWithHead();
        }

        public Size Size { get; private set; }
        public AOColor HeadColor { get; private set; }

        public ModuleRow[,] ModuleRows { get; set; }
        public ModuleRow this[int x, int y]
        {
            get
            {
                if (x > -1 && x < Size.X && y > -1 && y < Size.Y)
                {
                    return ModuleRows[x, y];
                }
                return null;
            }
        }
        public ModuleRow[] ToArray()
        {
            var rows = new List<ModuleRow>();

            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    rows.Add(ModuleRows[x, y]);

            return rows.ToArray();
        }

        public void SetUndefinied(int x, int y)
        {
            ModuleRows[x, y] = new ModuleRow(HeadColor);
        }

        public Move GetOwnHead()
        {
            return _ownHead;
        }

        public void SetOwnHead(int x, int y, AOColor aoColor, Direction direction)
        {
            if (_ownHead != null)
                ModuleRows[_ownHead.X, _ownHead.Y] = null;

            if (HeadColor != aoColor)
            {
                HeadColor = aoColor;
                PlaceUndefined();
            }

            ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnHead, Exclude.No, aoColor, _snakeId);
            _ownHead = new Move(x, y, direction);

        }

        public void SetOwnBody(int x, int y, Exclude exclude, AOColor aoColor)
        {
            ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnBody, exclude, aoColor, _snakeId);
        }

        public void SetOwnTail(int x, int y, Exclude exclude, AOColor aoColor)
        {
            ModuleRows[x, y] = new ModuleRow(ModuleRowContent.OwnTail, exclude, aoColor, _snakeId);
        }

        private void InitilaizeWithHead()
        {
            PlaceUndefined();

            SetOwnHead(Size.X / 2, Size.Y / 2, HeadColor, Direction.North);
        }

        private void PlaceUndefined()
        {
            for (int y = 0; y < Size.Y; y++)
                for (int x = 0; x < Size.X; x++)
                    if (ModuleRows[x, y] == null || ModuleRows[x, y].ModuleRowContent == ModuleRowContent.Undefined)
                        SetUndefinied(x, y);
        }
    }
}
