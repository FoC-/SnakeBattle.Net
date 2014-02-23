using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Core.Observers;

namespace SnakeBattleNet.Core
{
    public class Fighter
    {
        public string Id { get; private set; }
        public ICollection<View<ChipCell>> Chips { get; set; }
        public LinkedList<Move> BodyParts { get; private set; }

        private readonly List<IObserver> observers = new List<IObserver>();

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
            set
            {
                if (Head != null)
                    Notify(new ReplayEvent(Id, Head.Position, Content.Body, Head.Direction));

                BodyParts.AddFirst(value);
                Notify(new ReplayEvent(Id, Head.Position, Content.Head, Head.Direction));
            }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            Notify(new ReplayEvent(Id, Tail.Position, Content.Empty, Tail.Direction));
            BodyParts.RemoveLast();
            Notify(new ReplayEvent(Id, Tail.Position, Content.Tail, Tail.Direction));
        }

        public void MoveForward()
        {
            switch (Head.Direction)
            {
                case Direction.North:
                    Head = Move.ToNothFrom(Head.Position);
                    break;
                case Direction.West:
                    Head = Move.ToWestFrom(Head.Position);
                    break;
                case Direction.East:
                    Head = Move.ToEastFrom(Head.Position);
                    break;
                case Direction.South:
                    Head = Move.ToSouthFrom(Head.Position);
                    break;
            }
        }

        public void TryToBite(IEnumerable<Fighter> fighters, Move newHeadPosition)
        {
            var fighter = fighters.FirstOrDefault(f => f.Tail.Position == newHeadPosition.Position);
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

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        private void Notify(ReplayEvent replayEvent)
        {
            foreach (var observer in observers)
            {
                observer.Notify(replayEvent);
            }
        }
    }
}