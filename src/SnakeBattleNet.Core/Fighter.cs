using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public BattleField Field { get; private set; }
        public ICollection<IEnumerable<ChipCell>> Chips { get; private set; }
        public LinkedList<Move> BodyParts { get; private set; }

        public Fighter(string id, BattleField field, ICollection<IEnumerable<ChipCell>> chips)
        {
            this.Field = field;
            Id = id;
            Chips = chips;
            BodyParts = new LinkedList<Move>();
        }

        public int Length { get { return BodyParts.Count; } }
        public Move Head
        {
            get { return Length == 0 ? null : BodyParts.First(); }
            set
            {
                if (Head != null)
                    Field[Head.X, Head.Y] = Content.Body;

                BodyParts.AddFirst(value);
                Field[Head.X, Head.Y] = Content.Head;
            }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            Field[Tail.X, Tail.Y] = Content.Empty;
            BodyParts.RemoveLast();
            Field[Tail.X, Tail.Y] = Content.Tail;
        }

        public void GrowForward()
        {
            switch (Head.Direction)
            {
                case Direction.North:
                    Head = Move.ToNothFrom(Head);
                    break;
                case Direction.West:
                    Head = Move.ToWestFrom(Head);
                    break;
                case Direction.East:
                    Head = Move.ToEastFrom(Head);
                    break;
                case Direction.South:
                    Head = Move.ToSouthFrom(Head);
                    break;
            }
        }

        public void BiteMove(IEnumerable<Fighter> fighters, Move newHeadPosition)
        {
            var fighter = fighters.FirstOrDefault(f => f.Tail.Equals(newHeadPosition));
            if (fighter == null)
            {
                CutTail();
            }
            else
            {
                fighter.CutTail();
            }
            Head = newHeadPosition;
        }
    }
}