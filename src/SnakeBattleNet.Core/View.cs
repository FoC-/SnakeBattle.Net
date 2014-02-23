using System.Collections;
using System.Collections.Generic;
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
