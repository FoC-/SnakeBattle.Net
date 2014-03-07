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

        public Fighter(string id, IEnumerable<IDictionary<Position, ChipCell>> chips)
        {
            Id = id;
            Chips = chips.Select(chipCells =>
            {
                var view = new View<ChipCell>();
                chipCells.ToList().ForEach(cell => view[cell.Key] = cell.Value);
                return view;
            }).ToList();
            BodyParts = new LinkedList<Move>();
        }

        public int Length { get { return BodyParts.Count; } }
        public Move Head
        {
            get { return Length == 0 ? null : BodyParts.First(); }
            set
            {
                if (Head != null)
                    Notify(new Move { Content = Content.Body, Direction = Head.Direction, X = Head.X, Y = Head.Y });

                BodyParts.AddFirst(value);
                Notify(new Move { Content = Content.Head, Direction = Head.Direction, X = Head.X, Y = Head.Y });
            }
        }

        public Move Tail
        {
            get { return Length == 0 ? null : BodyParts.Last(); }
        }

        public void CutTail()
        {
            Notify(new Move { Content = Content.Empty, Direction = Tail.Direction, X = Tail.X, Y = Tail.Y });
            BodyParts.RemoveLast();
            Notify(new Move { Content = Content.Tail, Direction = Tail.Direction, X = Tail.X, Y = Tail.Y });
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

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        private void Notify(Move cell)
        {
            foreach (var observer in observers)
            {
                observer.Notify(cell);
            }
        }
    }
}