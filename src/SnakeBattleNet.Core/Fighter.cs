using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public LinkedList<Directed> BodyParts { get; private set; }
        public ICollection<IEnumerable<ChipCell>> Chips { get; private set; }

        public Directed Head
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.First(); }
            set { BodyParts.AddFirst(value); }
        }

        public Directed Tail
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.Last(); }
        }

        public Fighter(string id, ICollection<IEnumerable<ChipCell>> chips, Directed head)
        {
            Id = id;
            BodyParts = new LinkedList<Directed>();
            Chips = chips;
            Head = head;
        }

        public void CutTail()
        {
            BodyParts.RemoveLast();
        }

        public void Grow(Direction direction, int length = 1)
        {
            for (var i = 0; i < length; i++)
            {
                Head = Directed.ToDirection(Head, direction);
            }
        }
    }
}