using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    public class stubForRealBattle
    {
        protected static Fighter CreateSnakeStub(ICollection<View<ChipCell>> chips)
        {
            var snakeStub = new Fighter(Guid.NewGuid().ToString(), chips);
            snakeStub.Head = new Move { X = 5, Y = 4, Direction = Direction.South };
            snakeStub.Head = new Move { X = 5, Y = 5, Direction = Direction.West };
            snakeStub.Head = new Move { X = 4, Y = 5, Direction = Direction.North };
            snakeStub.Head = new Move { X = 4, Y = 4, Direction = Direction.North };
            return snakeStub;
        }

        protected static View<Content> CreateBattleField()
        {
            var battleField = new View<Content>();

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

        protected static View<ChipCell> CreateChipWithAndColoredHead()
        {
            var chip = new View<ChipCell>();

            // Own snake
            chip[new Position { X = 2, Y = 2 }] = new ChipCell { Content = Content.Head, Color = Color.AndGrey, IsSelf = true };
            chip[new Position { X = 2, Y = 3 }] = new ChipCell { Content = Content.Body, Color = Color.OrGreen, IsSelf = true };
            chip[new Position { X = 3, Y = 3 }] = new ChipCell { Content = Content.Body, Color = Color.OrGreen, IsSelf = true };
            chip[new Position { X = 3, Y = 2 }] = new ChipCell { Content = Content.Tail, Color = Color.OrGreen, IsSelf = true };

            // Snake 1
            chip[new Position { X = 2, Y = 1 }] = new ChipCell { Content = Content.Head, Color = Color.AndBlack };
            chip[new Position { X = 2, Y = 0 }] = new ChipCell { Content = Content.Body, Color = Color.AndBlack };
            chip[new Position { X = 3, Y = 0 }] = new ChipCell { Content = Content.Body, Color = Color.AndBlack };
            chip[new Position { X = 3, Y = 1 }] = new ChipCell { Content = Content.Tail, Color = Color.AndBlack };

            // Snake 2
            chip[new Position { X = 0, Y = 0 }] = new ChipCell { Content = Content.Head, Color = Color.AndGrey };
            chip[new Position { X = 1, Y = 0 }] = new ChipCell { Content = Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 1, Y = 1 }] = new ChipCell { Content = Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 0, Y = 1 }] = new ChipCell { Content = Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 0, Y = 2 }] = new ChipCell { Content = Content.Tail, Color = Color.AndGrey };

            // Snake 3
            chip[new Position { X = 0, Y = 3 }] = new ChipCell { Content = Content.Head, Color = Color.OrBlue };
            chip[new Position { X = 0, Y = 4 }] = new ChipCell { Content = Content.Body, Color = Color.OrBlue };

            return chip;
        }
    }
}