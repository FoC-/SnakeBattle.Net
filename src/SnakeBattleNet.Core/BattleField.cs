using System.Collections;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField : IEnumerable<KeyValuePair<Position, Content>>
    {
        private readonly IDictionary<Position, Content> _cells;

        public const int SideLength = 27;
        public const int GatewaysPerSide = 1;
        public IList<Move> Gateways { get; private set; }

        public Content this[Position position]
        {
            get
            {
                Content content;
                return _cells.TryGetValue(position, out content) ? content : Content.Empty;
            }
            set
            {
                Content content;
                if (_cells.TryGetValue(position, out content))
                {
                    _cells.Remove(position);
                }
                _cells.Add(position, value);
            }
        }

        public BattleField()
        {
            _cells = new Dictionary<Position, Content>();
            Gateways = new List<Move>();
            CreateEmpty();
            CreateWalls();
            CreateGateways();
        }

        public IDictionary<Position, Content> ViewToNorth(Position field, Position chip, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    cells.Add(new Position { X = rx, Y = ry }, this[new Position { X = x, Y = y }]);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToWest(Position field, Position chip, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    cells.Add(new Position { X = rx, Y = ry }, this[new Position { X = x, Y = y }]);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToEast(Position field, Position chip, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    cells.Add(new Position { X = rx, Y = ry }, this[new Position { X = x, Y = y }]);
                }
            return cells;
        }

        public IDictionary<Position, Content> ViewToSouth(Position field, Position chip, int chipSideLength)
        {
            var cells = new Dictionary<Position, Content>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    cells.Add(new Position { X = rx, Y = ry }, this[new Position { X = x, Y = y }]);
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

        IEnumerator<KeyValuePair<Position, Content>> IEnumerable<KeyValuePair<Position, Content>>.GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _cells.GetEnumerator();
        }
    }
}
