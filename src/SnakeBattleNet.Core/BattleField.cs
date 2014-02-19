using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField
    {
        public int SideLength { get; private set; }
        public int GatewaysPerSide { get; private set; }
        public IEnumerable<FieldCell> Cells { get; private set; }
        public IList<Move> Gateways { get; private set; }

        public BattleField()
        {
            SideLength = 27;
            GatewaysPerSide = 1;
            Cells = Enumerable.Empty<FieldCell>();
            Gateways = new List<Move>();
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
                    cell = cell ?? new FieldCell { FieldContent = Content.Empty };
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
                    cell = cell ?? new FieldCell { FieldContent = Content.Empty };
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
                    cell = cell ?? new FieldCell { FieldContent = Content.Empty };
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
                    cell = cell ?? new FieldCell { FieldContent = Content.Empty };
                    cell.Position = new Position { X = i, Y = j };
                    rows.Add(cell);
                }
            return rows;
        }

        private void CreateGateways()
        {
            int x = SideLength / (GatewaysPerSide + 1);
            int y = SideLength / (GatewaysPerSide + 1);

            for (int i = 1; i < GatewaysPerSide + 1; i++)
            {
                Gateways.Add(new Move(0, i * y, Direction.East));
                Gateways.Add(new Move(SideLength - 1, i * y, Direction.West));
                Gateways.Add(new Move(i * x, 0, Direction.North));
                Gateways.Add(new Move(i * x, SideLength - 1, Direction.South));
            }
        }
    }
}
