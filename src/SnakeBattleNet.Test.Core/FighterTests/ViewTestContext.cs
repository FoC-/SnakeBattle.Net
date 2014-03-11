using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.FighterTests
{
    internal class ViewTestContext
    {
        internal static Fighter CreateFighter()
        {
            var fighter = new Fighter("id", new BattleField(), new List<IEnumerable<ChipCell>>());
            fighter.Head = new Move { X = 25, Y = 25, Direction = Direction.North };
            return fighter;
        }

        internal static Fighter CreateSnakeStub(ICollection<IEnumerable<ChipCell>> chips, BattleField battleField)
        {
            var snakeStub = new Fighter(Guid.NewGuid().ToString(), battleField, chips);
            snakeStub.Head = new Move { X = 5, Y = 4, Direction = Direction.South };
            snakeStub.Head = new Move { X = 5, Y = 5, Direction = Direction.West };
            snakeStub.Head = new Move { X = 4, Y = 5, Direction = Direction.North };
            snakeStub.Head = new Move { X = 4, Y = 4, Direction = Direction.North };
            return snakeStub;
        }

        internal static BattleField CreateBattleField()
        {
            var battleField = new BattleField();

            // Snake 0
            battleField[4, 4] = Content.Head;
            battleField[4, 5] = Content.Body;
            battleField[5, 5] = Content.Body;
            battleField[5, 4] = Content.Tail;

            // Snake 1
            battleField[4, 3] = Content.Head;
            battleField[4, 2] = Content.Body;
            battleField[5, 2] = Content.Body;
            battleField[5, 3] = Content.Tail;

            // Snake 2
            battleField[2, 2] = Content.Head;
            battleField[3, 2] = Content.Body;
            battleField[3, 3] = Content.Body;
            battleField[2, 3] = Content.Body;
            battleField[2, 4] = Content.Tail;

            // Snake 3
            battleField[2, 5] = Content.Head;
            battleField[2, 6] = Content.Body;
            battleField[2, 7] = Content.Tail;

            return battleField;
        }

        internal static IEnumerable<ChipCell> CreateChipWithAndColoredHead()
        {
            var chip = new List<ChipCell>();

            // Own snake
            chip.Add(new ChipCell { X = 2, Y = 2, Content = Content.Head, Color = Color.AndGrey, IsSelf = true });
            chip.Add(new ChipCell { X = 2, Y = 3, Content = Content.Body, Color = Color.OrGreen, IsSelf = true });
            chip.Add(new ChipCell { X = 3, Y = 3, Content = Content.Body, Color = Color.OrGreen, IsSelf = true });
            chip.Add(new ChipCell { X = 3, Y = 2, Content = Content.Tail, Color = Color.OrGreen, IsSelf = true });

            // Snake 1
            chip.Add(new ChipCell { X = 2, Y = 1, Content = Content.Head, Color = Color.AndBlack });
            chip.Add(new ChipCell { X = 2, Y = 0, Content = Content.Body, Color = Color.AndBlack });
            chip.Add(new ChipCell { X = 3, Y = 0, Content = Content.Body, Color = Color.AndBlack });
            chip.Add(new ChipCell { X = 3, Y = 1, Content = Content.Tail, Color = Color.AndBlack });

            // Snake 2
            chip.Add(new ChipCell { X = 0, Y = 0, Content = Content.Head, Color = Color.AndGrey });
            chip.Add(new ChipCell { X = 1, Y = 0, Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new ChipCell { X = 1, Y = 1, Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new ChipCell { X = 0, Y = 1, Content = Content.Body, Color = Color.AndGrey });
            chip.Add(new ChipCell { X = 0, Y = 2, Content = Content.Tail, Color = Color.AndGrey });

            // Snake 3
            chip.Add(new ChipCell { X = 0, Y = 3, Content = Content.Head, Color = Color.OrBlue });
            chip.Add(new ChipCell { X = 0, Y = 4, Content = Content.Body, Color = Color.OrBlue });

            return chip;
        }
    }
}