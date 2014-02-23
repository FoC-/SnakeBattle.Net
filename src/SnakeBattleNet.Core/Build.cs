using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Core
{
    public static class Build
    {
        public const int BattleFieldSideLength = 27;

        public static View<Content> BattleField()
        {
            var battleField = new View<Content>();
            CreateEmpty(battleField);
            CreateWalls(battleField);
            return battleField;
        }

        private static void CreateEmpty(View<Content> battleField)
        {
            for (var x = 1; x < BattleFieldSideLength - 1; x++)
                for (var y = 1; y < BattleFieldSideLength - 1; y++)
                {
                    battleField[new Position { X = x, Y = y }] = Content.Empty;
                }
        }

        private static void CreateWalls(View<Content> battleField)
        {
            for (var x = 0; x < BattleFieldSideLength; x++)
            {
                battleField[new Position { X = x, Y = 0 }] = Content.Wall;
                battleField[new Position { X = x, Y = BattleFieldSideLength - 1 }] = Content.Wall;
            }
            for (var y = 0; y < BattleFieldSideLength; y++)
            {
                battleField[new Position { X = 0, Y = y }] = Content.Wall;
                battleField[new Position { X = BattleFieldSideLength - 1, Y = y }] = Content.Wall;
            }
        }
    }
}