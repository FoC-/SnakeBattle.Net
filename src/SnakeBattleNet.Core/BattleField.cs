using System;
using System.Text;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField
    {
        public int SideLength { get; private set; }
        private readonly Content[,] field;

        public Content this[int x, int y]
        {
            get
            {
                try
                {
                    return field[x, y];
                }
                catch
                {
                    return Content.Empty;
                }
            }
            set { field[x, y] = value; }
        }

        public BattleField()
        {
            SideLength = 27;
            field = new Content[SideLength, SideLength];
            CreateEmpty();
            CreateWalls();
        }

        public Cell<Content> RelativeCell(Direction direction, Position fieldHead, Position chipHead, Position chipCell)
        {
            switch (direction)
            {
                case Direction.North:
                    return NorthView(fieldHead, chipHead, chipCell);
                case Direction.West:
                    return WestView(fieldHead, chipHead, chipCell);
                case Direction.East:
                    return EastView(fieldHead, chipHead, chipCell);
                case Direction.South:
                    return SouthView(fieldHead, chipHead, chipCell);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private Cell<Content> NorthView(Position fieldHead, Position chipHead, Position chipCell)
        {
            var x = fieldHead.X + chipCell.X - chipHead.X;
            var y = fieldHead.Y + chipCell.Y - chipHead.Y;
            return new Cell<Content> { X = x, Y = y, Content = field[x, y] };
        }

        private Cell<Content> WestView(Position fieldHead, Position chipHead, Position chipCell)
        {
            var x = fieldHead.X - chipCell.Y + chipHead.Y;
            var y = fieldHead.Y + chipCell.X - chipHead.X;
            return new Cell<Content> { X = x, Y = y, Content = field[x, y] };
        }

        private Cell<Content> EastView(Position fieldHead, Position chipHead, Position chipCell)
        {
            var x = fieldHead.X + chipCell.Y - chipHead.Y;
            var y = fieldHead.Y - chipCell.X + chipHead.X;
            return new Cell<Content> { X = x, Y = y, Content = field[x, y] };
        }

        private Cell<Content> SouthView(Position fieldHead, Position chipHead, Position chipCell)
        {
            var x = fieldHead.X - chipCell.X + chipHead.X;
            var y = fieldHead.Y - chipCell.Y + chipHead.Y;
            return new Cell<Content> { X = x, Y = y, Content = field[x, y] };
        }

        private void CreateEmpty()
        {
            for (var x = 1; x < SideLength - 1; x++)
                for (var y = 1; y < SideLength - 1; y++)
                {
                    field[x, y] = Content.Empty;
                }
        }

        private void CreateWalls()
        {
            for (var x = 0; x < SideLength; x++)
            {
                field[x, 0] = Content.Wall;
                field[x, SideLength - 1] = Content.Wall;
            }
            for (var y = 0; y < SideLength; y++)
            {
                field[0, y] = Content.Wall;
                field[SideLength - 1, y] = Content.Wall;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var y = SideLength - 1; y >= 0; y--)
            {
                for (var x = 0; x < SideLength; x++)
                {
                    builder.Append(field[x, y].ToString().Substring(0, 1));
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}