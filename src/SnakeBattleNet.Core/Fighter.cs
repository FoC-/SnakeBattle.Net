using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        private readonly BattleField field;

        public string Id { get; private set; }
        public LinkedList<Directed> BodyParts { get; private set; }
        public ICollection<IEnumerable<ChipCell>> Chips { get; private set; }

        public Directed Head
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.First(); }
            set
            {
                if (Head != null)
                    field[Head.X, Head.Y] = Content.Body;

                BodyParts.AddFirst(value);
                field[Head.X, Head.Y] = Content.Head;
            }
        }

        public Directed Tail
        {
            get { return BodyParts.Count == 0 ? null : BodyParts.Last(); }
        }

        public Fighter(string id, BattleField field, ICollection<IEnumerable<ChipCell>> chips, Directed head)
        {
            Id = id;
            BodyParts = new LinkedList<Directed>();
            this.field = field;
            Chips = chips;
            Head = head;
        }

        public void CutTail()
        {
            // todo: dead
            field[Tail.X, Tail.Y] = Content.Empty;
            BodyParts.RemoveLast();
            if (BodyParts.Count > 1)
                field[Tail.X, Tail.Y] = Content.Tail;
        }

        public void Grow(Direction direction, int length = 1)
        {
            for (var i = 0; i < length; i++)
            {
                Head = Directed.ToDirection(Head, direction);
            }
            field[Tail.X, Tail.Y] = Content.Tail;
        }
    }
}