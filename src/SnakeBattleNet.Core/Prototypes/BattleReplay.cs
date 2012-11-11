using System;

namespace SnakeBattleNet.Core.Prototypes
{
    public class BattleReplay
    {
    }

    public abstract class GameEvent
    {
        public DateTime Time { get; set; }
    }

    //snake placed on a battle field
    public class SnakePlaced : GameEvent
    {
        public Snake Snake { get; set; }
        public Coordinates Head { get; set; }
        public Coordinates Tail { get; set; }
    }

    //snake moved
    public class SnakeMoved : GameEvent
    {
        public Snake Snake { get; set; }
        public Coordinates Coordinates { get; set; }
    }

    //snake bit other snake
    public class TailWasBitten : GameEvent
    {
        public Snake AttackingSnake { get; set; }
        public Snake TargetSnake { get; set; }
    }

    public class ReplayRecord<T> where T : GameEvent
    {
        public Snake Snake { get; set; }
        public T GameEvent { get; set; }
    }
}