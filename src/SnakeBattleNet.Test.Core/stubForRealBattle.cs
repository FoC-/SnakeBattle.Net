using System;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    public class stubForRealBattle
    {
        protected static Snake CreateSnakeStub()
        {
            var snakeStub = new Snake(Guid.NewGuid().ToString());
            snakeStub.Head = new Move(5, 4, Direction.South);
            snakeStub.Head = new Move(5, 5, Direction.West);
            snakeStub.Head = new Move(4, 5, Direction.North);
            snakeStub.Head = new Move(4, 4, Direction.North);
            return snakeStub;
        }

        protected static BattleField CreateBattleField(string idForOwnSnake)
        {
            var battleField = new BattleField(new Size(9, 9), 1, "field-id");

            // Snake 0
            battleField[4, 4] = new FieldRow(FieldContent.Head, idForOwnSnake);
            battleField[4, 5] = new FieldRow(FieldContent.Body, idForOwnSnake);
            battleField[5, 5] = new FieldRow(FieldContent.Body, idForOwnSnake);
            battleField[5, 4] = new FieldRow(FieldContent.Tail, idForOwnSnake);

            // Snake 1
            var idForSnake1 = Guid.NewGuid().ToString();
            battleField[4, 3] = new FieldRow(FieldContent.Head, idForSnake1);
            battleField[4, 2] = new FieldRow(FieldContent.Body, idForSnake1);
            battleField[5, 2] = new FieldRow(FieldContent.Body, idForSnake1);
            battleField[5, 3] = new FieldRow(FieldContent.Tail, idForSnake1);

            // Snake 2
            var idForSnake2 = Guid.NewGuid().ToString();
            battleField[2, 2] = new FieldRow(FieldContent.Head, idForSnake2);
            battleField[3, 2] = new FieldRow(FieldContent.Body, idForSnake2);
            battleField[3, 3] = new FieldRow(FieldContent.Body, idForSnake2);
            battleField[2, 3] = new FieldRow(FieldContent.Body, idForSnake2);
            battleField[2, 4] = new FieldRow(FieldContent.Tail, idForSnake2);

            // Snake 3
            var idForSnake3 = Guid.NewGuid().ToString();
            battleField[2, 5] = new FieldRow(FieldContent.Head, idForSnake3);
            battleField[2, 6] = new FieldRow(FieldContent.Body, idForSnake3);
            battleField[2, 7] = new FieldRow(FieldContent.Tail, idForSnake3);

            return battleField;
        }

        protected static BrainModule CreateChipWithAndColoredHead(string snakeId)
        {
            var chipWithAndColoredHead = new BrainModule(new Size(5, 5), snakeId);

            // Own snake
            chipWithAndColoredHead.SetOwnHead(2, 2, AOColor.AndGrey, Direction.North);
            chipWithAndColoredHead.SetOwnBody(2, 3, Exclude.No, AOColor.OrGreen);
            chipWithAndColoredHead.SetOwnBody(3, 3, Exclude.No, AOColor.OrGreen);
            chipWithAndColoredHead.SetOwnTail(3, 2, Exclude.No, AOColor.OrGreen);

            // Snake 1
            chipWithAndColoredHead.ModuleRows[2, 1] = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.ModuleRows[2, 0] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.ModuleRows[3, 0] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.ModuleRows[3, 1] = new ModuleRow(ModuleRowContent.EnemyTail, Exclude.No, AOColor.AndBlack);

            // Snake 2
            chipWithAndColoredHead.ModuleRows[0, 0] = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.ModuleRows[1, 0] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.ModuleRows[1, 1] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.ModuleRows[0, 1] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.ModuleRows[0, 2] = new ModuleRow(ModuleRowContent.EnemyTail, Exclude.No, AOColor.AndGrey);

            // Snake 3
            chipWithAndColoredHead.ModuleRows[0, 3] = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.No, AOColor.OrBlue);
            chipWithAndColoredHead.ModuleRows[0, 4] = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.OrBlue);

            return chipWithAndColoredHead;
        }
    }
}