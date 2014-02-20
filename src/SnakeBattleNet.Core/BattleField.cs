using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField
    {
        public int SideLength { get; private set; }
        public int GatewaysPerSide { get; private set; }
        public IDictionary<Position, Content> Cells { get; private set; }
        public IList<Move> Gateways { get; private set; }

        public Content this[Position position]
        {
            get
            {
                Content content;
                return Cells.TryGetValue(position, out content) ? content : Content.Empty;
            }
            set
            {
                Content content;
                if (Cells.TryGetValue(position, out content))
                {
                    Cells.Remove(position);
                }
                Cells.Add(position, value);
            }
        }

        public BattleField()
        {
            SideLength = 27;
            GatewaysPerSide = 1;
            Cells = new Dictionary<Position, Content>();
            Gateways = new List<Move>();
            CreateEmpty();
            CreateWalls();
            CreateGateways();
        }

        public IDictionary<Position, Content> ViewToNorth(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int j = 0, y = fy - cy; y < fy - cy + chipSideLength; y++, j++)
                for (int i = 0, x = fx - cx; x < fx - cx + chipSideLength; x++, i++)
                {
                    var position = new Position { X = i, Y = j };
                    var content = this[position];
                    cells.Add(position, content);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToWest(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int j = 0, y = fy + cy; y > fy + cy - chipSideLength; y--, j++)
                for (int i = 0, x = fx - cx; x < fx - cx + chipSideLength; x++, i++)
                {
                    var position = new Position { X = i, Y = j };
                    var content = this[position];
                    cells.Add(position, content);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToEast(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int j = 0, y = fy - cy; y < fy - cy + chipSideLength; y++, j++)
                for (int i = 0, x = fx + cx; x > fx + cx - chipSideLength; x--, i++)
                {
                    var position = new Position { X = i, Y = j };
                    var content = this[position];
                    cells.Add(position, content);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToSouth(Position snakeHeadPositionOnBattleField, Position snakeHeadPositionOnModule, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            int fx = snakeHeadPositionOnBattleField.X;
            int fy = snakeHeadPositionOnBattleField.Y;

            int cx = snakeHeadPositionOnModule.X;
            int cy = snakeHeadPositionOnModule.Y;

            for (int j = 0, y = fy + cy; y > fy + cy - chipSideLength; y--, j++)
                for (int i = 0, x = fx + cx; x > fx + cx - chipSideLength; x--, i++)
                {
                    var position = new Position { X = i, Y = j };
                    var content = this[position];
                    cells.Add(position, content);
                }
            return cells;
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
