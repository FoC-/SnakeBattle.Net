using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField
    {
        public int SideLength { get; private set; }
        public int GatewaysPerSide { get; private set; }
        public ICollection<FieldCell> Cells { get; private set; }
        public IList<Move> Gateways { get; private set; }

        public FieldCell this[int x, int y]
        {
            get
            {
                var cell = Cells.FirstOrDefault(c => c.Position.X == x && c.Position.Y == y);
                return cell ?? new FieldCell { Content = Content.Empty, Position = new Position { X = x, Y = y } };
            }
        }

        public Content this[Position position]
        {
            set
            {
                var tempCells = Cells.ToList();
                tempCells.RemoveAll(c => c.Position.X == position.X && c.Position.Y == position.Y);
                tempCells.Add(new FieldCell { Content = value, Position = position });
                Cells = tempCells;
            }
        }

        public BattleField()
        {
            SideLength = 27;
            GatewaysPerSide = 1;
            Cells = new List<FieldCell>();
            Gateways = new List<Move>();
            CreateEmpty();
            CreateWalls();
            CreateGateways();
        }

        public IEnumerable<FieldCell> ViewToNorth(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var rows = new List<FieldCell>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            int i = 0, j = 0;
            for (int y = fy - cy; y < fy - cy + chipSideLength; y++, j++)
                for (int x = fx - cx; x < fx - cx + chipSideLength; x++, i++)
                {
                    var cell = Cells.FirstOrDefault(c => c.Position.X == x && c.Position.Y == y);
                    cell = cell ?? new FieldCell { Content = Content.Empty };
                    cell.Position = new Position { X = i, Y = j };
                    rows.Add(cell);
                }
            return rows;
        }

        public IEnumerable<FieldCell> ViewToWest(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var rows = new List<FieldCell>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            int i = 0, j = 0;
            for (int y = fy + cy; y > fy + cy - chipSideLength; y--, j++)
                for (int x = fx - cx; x < fx - cx + chipSideLength; x++, i++)
                {
                    var cell = Cells.FirstOrDefault(c => c.Position.X == x && c.Position.Y == y);
                    cell = cell ?? new FieldCell { Content = Content.Empty };
                    cell.Position = new Position { X = i, Y = j };
                    rows.Add(cell);
                }
            return rows;
        }

        public IEnumerable<FieldCell> ViewToEast(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var rows = new List<FieldCell>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            int i = 0, j = 0;
            for (int y = fy - cy; y < fy - cy + chipSideLength; y++, j++)
                for (int x = fx + cx; x > fx + cx - chipSideLength; x--, i++)
                {
                    var cell = Cells.FirstOrDefault(c => c.Position.X == x && c.Position.Y == y);
                    cell = cell ?? new FieldCell { Content = Content.Empty };
                    cell.Position = new Position { X = i, Y = j };
                    rows.Add(cell);
                }
            return rows;
        }

        public IEnumerable<FieldCell> ViewToSouth(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var rows = new List<FieldCell>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            int i = 0, j = 0;
            for (int y = fy + cy; y > fy + cy - chipSideLength; y--, j++)
                for (int x = fx + cx; x > fx + cx - chipSideLength; x--, i++)
                {
                    var cell = Cells.FirstOrDefault(c => c.Position.X == x && c.Position.Y == y);
                    cell = cell ?? new FieldCell { Content = Content.Empty };
                    cell.Position = new Position { X = i, Y = j };
                    rows.Add(cell);
                }
            return rows;
        }

        private void CreateEmpty()
        {
            for (var x = 1; x < SideLength - 1; x++)
                for (var y = 1; y < SideLength - 1; y++)
                {
                    this[new Position { X = x, Y = y }] = Content.Empty;
                }
        }

        private void CreateWalls()
        {
            for (var x = 0; x < SideLength; x++)
            {
                this[new Position { X = x, Y = 0 }] = Content.Wall;
                this[new Position { X = x, Y = SideLength - 1 }] = Content.Wall;
            }
            for (var y = 0; y < SideLength; y++)
            {
                this[new Position { X = 0, Y = y }] = Content.Wall;
                this[new Position { X = SideLength - 1, Y = y }] = Content.Wall;
            }
        }

        private void CreateGateways()
        {
            var x = SideLength / (GatewaysPerSide + 1);
            var y = SideLength / (GatewaysPerSide + 1);

            for (var i = 1; i < GatewaysPerSide + 1; i++)
            {
                Gateways.Add(new Move(0, i * y, Direction.East));
                Gateways.Add(new Move(SideLength - 1, i * y, Direction.West));
                Gateways.Add(new Move(i * x, 0, Direction.North));
                Gateways.Add(new Move(i * x, SideLength - 1, Direction.South));
            }
        }
    }
}
