using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public ICollection<View<ChipCell>> Chips { get; set; }
        public LinkedList<Move> BodyParts { get; private set; }

        public Fighter(string id, ICollection<View<ChipCell>> chips)
        {
            Id = id;
            Chips = chips;
            BodyParts = new LinkedList<Move>();
        }

        public int Length { get { return BodyParts.Count; } }
        public Move Head
        {
            get { return Length == 0 ? null : BodyParts.First(); }
            set { BodyParts.AddFirst(value); }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            BodyParts.RemoveLast();
        }
    }
}