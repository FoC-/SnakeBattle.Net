using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core.Observers
{
    public class BattleField : View<Content>, IObserver
    {
        public const int SideLength = 27;

        public static BattleField Build()
        {
            var battleField = new BattleField();
            CreateEmpty(battleField);
            CreateWalls(battleField);
            return battleField;
        }

        private static void CreateEmpty(View<Content> battleField)
        {
            for (var x = 1; x < SideLength - 1; x++)
                for (var y = 1; y < SideLength - 1; y++)
                {
                    battleField[new Position { X = x, Y = y }] = Content.Empty;
                }
        }

        private static void CreateWalls(View<Content> battleField)
        {
            for (var x = 0; x < SideLength; x++)
            {
                battleField[new Position { X = x, Y = 0 }] = Content.Wall;
                battleField[new Position { X = x, Y = SideLength - 1 }] = Content.Wall;
            }
            for (var y = 0; y < SideLength; y++)
            {
                battleField[new Position { X = 0, Y = y }] = Content.Wall;
                battleField[new Position { X = SideLength - 1, Y = y }] = Content.Wall;
            }
        }

        public void Notify(ReplayEvent replayEvent)
        {
            this[replayEvent.P] = replayEvent.C;
        }
    }
}