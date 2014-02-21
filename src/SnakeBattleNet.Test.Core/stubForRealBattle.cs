using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    public class stubForRealBattle
    {
        protected static Snake CreateSnakeStub()
        {
            var snakeStub = new Snake(Guid.NewGuid().ToString());
            snakeStub.Head = new Move(new Position { X = 5, Y = 4 }, Direction.South);
            snakeStub.Head = new Move(new Position { X = 5, Y = 5 }, Direction.West);
            snakeStub.Head = new Move(new Position { X = 4, Y = 5 }, Direction.North);
            snakeStub.Head = new Move(new Position { X = 4, Y = 4 }, Direction.North);
            return snakeStub;
        }

        protected static BattleField CreateBattleField()
        {
            var battleField = new BattleField();

            // Snake 0
            battleField[new Position { X = 4, Y = 4 }] = Content.Head;
            battleField[new Position { X = 4, Y = 5 }] = Content.Body;
            battleField[new Position { X = 5, Y = 5 }] = Content.Body;
            battleField[new Position { X = 5, Y = 4 }] = Content.Tail;

            // Snake 1
            battleField[new Position { X = 4, Y = 3 }] = Content.Head;
            battleField[new Position { X = 4, Y = 2 }] = Content.Body;
            battleField[new Position { X = 5, Y = 2 }] = Content.Body;
            battleField[new Position { X = 5, Y = 3 }] = Content.Tail;

            // Snake 2
            battleField[new Position { X = 2, Y = 2 }] = Content.Head;
            battleField[new Position { X = 3, Y = 2 }] = Content.Body;
            battleField[new Position { X = 3, Y = 3 }] = Content.Body;
            battleField[new Position { X = 2, Y = 3 }] = Content.Body;
            battleField[new Position { X = 2, Y = 4 }] = Content.Tail;

            // Snake 3
            battleField[new Position { X = 2, Y = 5 }] = Content.Head;
            battleField[new Position { X = 2, Y = 6 }] = Content.Body;
            battleField[new Position { X = 2, Y = 7 }] = Content.Tail;

            return battleField;
        }

        protected static IDictionary<Position, ChipCell> CreateChipWithAndColoredHead()
        {
            var chip = new Dictionary<Position, ChipCell>();

            // Own snake
            chip.Add(new Position { X = 2, Y = 2 }, new ChipCell { Content = Content.Head, Color = Color.AndGrey, IsSelf = true });
            chip.Add(new Position { X = 2, Y = 3 }, new ChipCell { Content = Content.Body, Color = Color.OrGreen, IsSelf = true });
            chip.Add(new Position { X = 3, Y = 3 }, new ChipCell { Content = Content.Body, Color = Color.OrGreen, IsSelf = true });
            chip.Add(new Position { X = 3, Y = 2 }, new ChipCell { Content = Content.Tail, Color = Color.OrGreen, IsSelf = true });

            // Snake 1
            chip.Add(new Position { X = 2, Y = 1 }, new ChipCell { Content = Content.Head, Color = Color.AndBlack });
            chip.Add(new Position { X = 2, Y = 0 }, new ChipCell { Content = Content.Body, Color = Color.AndBlack });
            chip.Add(new Position { X = 3, Y = 0 }, new ChipCell { Content = Content.Body, Color = Color.AndBlack });
            chip.Add(new Position { X = 3, Y = 1 }, new ChipCell { Content = Content.Tail, Color = Color.AndBlack });

            // Snake 2
            chip.Add(new Position { X = 0, Y = 0 }, new ChipCell { Content = Content.Head, Color = Color.AndGrey });
            chip.Add(new Position { X = 1, Y = 0 }, new ChipCell { Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new Position { X = 1, Y = 1 }, new ChipCell { Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new Position { X = 0, Y = 1 }, new ChipCell { Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new Position { X = 0, Y = 2 }, new ChipCell { Content = Content.Tail, Color = Color.AndGrey });

            // Snake 3
            chip.Add(new Position { X = 0, Y = 3 }, new ChipCell { Content = Content.Head, Color = Color.OrBlue });
            chip.Add(new Position { X = 0, Y = 4 }, new ChipCell { Content = Content.Body, Color = Color.OrBlue });

            return chip;
        }
    }
}