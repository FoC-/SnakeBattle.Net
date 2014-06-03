using System.Collections.Generic;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public ICollection<IEnumerable<ChipCell>> Chips { get; private set; }

        public Directed Head;
        public LinkedList<Directed> Body { get; private set; }
        public Directed Tail;

        public Fighter(string id, ICollection<IEnumerable<ChipCell>> chips, Directed tail)
        {
            Id = id;
            Body = new LinkedList<Directed>();
            Chips = chips;
            Tail = tail;
        }

        public void Grow(Direction direction)
        {
            if (Body.First == null && Tail == null) return;
            if (Body.First == null && Head == null)
            {
                Head = Directed.ToDirection(Tail, direction);
                return;
            }
            if (Head != null)
            {
                Body.AddFirst(Head);
            }
            Head = Directed.ToDirection(Body.First.Value, direction);
        }

        public void CutTail()
        {
            if (Body.Count == 0)
            {
                Tail = Head;
            }
            else
            {
                Tail = Body.Last.Value;
                Body.RemoveLast();
            }
        }
    }
}