using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class View<TContent> : IEnumerable<KeyValuePair<Position, TContent>>
    {
        private readonly IDictionary<Position, TContent> elements = new Dictionary<Position, TContent>();

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

        public View<Tuple<TContent, bool>> ToNorth(Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head.Position;
            var view = new View<Tuple<TContent, bool>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<TContent, bool>(this[absolutePostion], fighter.BodyParts.Any(m => m.Position == absolutePostion));
                }
            return view;
        }

        public View<Tuple<TContent, bool>> ToWest(Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head.Position;
            var view = new View<Tuple<TContent, bool>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X - chip.X; x < field.X - chip.X + chipSideLength; x++, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<TContent, bool>(this[absolutePostion], fighter.BodyParts.Any(m => m.Position == absolutePostion));
                }
            return view;
        }

        public View<Tuple<TContent, bool>> ToEast(Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head.Position;
            var view = new View<Tuple<TContent, bool>>();
            for (int ry = 0, y = field.Y - chip.Y; y < field.Y - chip.Y + chipSideLength; y++, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<TContent, bool>(this[absolutePostion], fighter.BodyParts.Any(m => m.Position == absolutePostion));
                }
            return view;
        }

        public View<Tuple<TContent, bool>> ToSouth(Fighter fighter, Position chip, int chipSideLength = 7)
        {
            var field = fighter.Head.Position;
            var view = new View<Tuple<TContent, bool>>();
            for (int ry = 0, y = field.Y + chip.Y; y > field.Y + chip.Y - chipSideLength; y--, ry++)
                for (int rx = 0, x = field.X + chip.X; x > field.X + chip.X - chipSideLength; x--, rx++)
                {
                    var absolutePostion = new Position { X = x, Y = y };
                    view[new Position { X = rx, Y = ry }] = new Tuple<TContent, bool>(this[absolutePostion], fighter.BodyParts.Any(m => m.Position == absolutePostion));
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
}
