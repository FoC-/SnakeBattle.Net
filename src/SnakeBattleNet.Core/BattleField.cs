using System;
using System.Collections;
using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class BattleField<TContent> : IEnumerable<KeyValuePair<Position, TContent>>
    {
        private readonly IDictionary<Position, TContent> elements;

        public TContent this[Position position]
        {
            get
            {
                TContent content;
                return elements.TryGetValue(position, out content) ? content : default(TContent);
            }
            set
            {
                TContent content;
                if (elements.TryGetValue(position, out content))
                {
                    elements.Remove(position);
                }
                elements.Add(position, value);
            }
        }

        public BattleField()
        {
            elements = new Dictionary<Position, TContent>();
        }

        public BattleField<Tuple<Position, TContent>> ViewToNorth(Position field, Position chip, int chipSideLength = 7)
        {
            var view = new BattleField<Tuple<Position, TContent>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<Position, TContent>(absolutePostion, this[absolutePostion]);
                }
            return view;
        }

        public BattleField<Tuple<Position, TContent>> ViewToWest(Position field, Position chip, int chipSideLength = 7)
        {
            var view = new BattleField<Tuple<Position, TContent>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<Position, TContent>(absolutePostion, this[absolutePostion]);
                }
            return view;
        }

        public BattleField<Tuple<Position, TContent>> ViewToEast(Position field, Position chip, int chipSideLength = 7)
        {
            var view = new BattleField<Tuple<Position, TContent>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<Position, TContent>(absolutePostion, this[absolutePostion]);
                }
            return view;
        }

        public BattleField<Tuple<Position, TContent>> ViewToSouth(Position field, Position chip, int chipSideLength = 7)
        {
            var view = new BattleField<Tuple<Position, TContent>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<Position, TContent>(absolutePostion, this[absolutePostion]);
                }
            return view;
        }

        IEnumerator<KeyValuePair<Position, TContent>> IEnumerable<KeyValuePair<Position, TContent>>.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }

    public static class BattleField
    {
        public const int SideLength = 27;
        public const int GatewaysPerSide = 1;
        public static Move[] Gateways = CreateGateways();

        public static BattleField<Content> Build()
        {
            var battleField = new BattleField<Content>();
            CreateEmpty(battleField);
            CreateWalls(battleField);
            return battleField;
        }

        private static void CreateEmpty(BattleField<Content> battleField)
        {
            for (var x = 1; x < SideLength - 1; x++)
                for (var y = 1; y < SideLength - 1; y++)
                {
                    battleField[new Position { X = x, Y = y }] = Content.Empty;
                }
        }

        private static void CreateWalls(BattleField<Content> battleField)
        {
            for (var x = 0; x < SideLength; x++)
            {
                battleField[new Position { X = x, Y = 0 }] = Content.Wall;
                battleField[new Position { X = x, Y = SideLength - 1 }] = Content.Wall;
            }
            for (var y = 0; y < SideLength; y++)
            {
                battleField[new Position { X = 0, Y = y }] = Content.Wall;
                battleField[new Position { X = SideLength - 1, Y = y }] = Content.Wall;
            }
        }

        private static Move[] CreateGateways()
        {
            var moves = new List<Move>();
            const int x = SideLength / (GatewaysPerSide + 1);
            const int y = SideLength / (GatewaysPerSide + 1);

            for (var i = 1; i < GatewaysPerSide + 1; i++)
            {
                moves.Add(new Move(new Position { X = 0, Y = i * y }, Direction.East));
                moves.Add(new Move(new Position { X = SideLength - 1, Y = i * y }, Direction.West));
                moves.Add(new Move(new Position { X = i * x, Y = 0 }, Direction.North));
                moves.Add(new Move(new Position { X = i * x, Y = SideLength - 1 }, Direction.South));
            }
            return moves.ToArray();
        }
    }
}
