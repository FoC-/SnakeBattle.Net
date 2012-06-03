using System;
using System.Collections.Generic;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;

namespace SnakeBattleNet.Test.Core
{
    public class stubForRealBattle
    {
        protected static ISnake CreateSnakeStub(IList<IBrainChip> brainChips, Guid idForOwnSnake)
        {
            var snakeStub = new Snake(id: idForOwnSnake, owner: Guid.NewGuid(), name: "name", brainModules: brainChips);
            snakeStub.SetHead(new Move(5, 4, Direction.South));
            snakeStub.SetHead(new Move(5, 5, Direction.West));
            snakeStub.SetHead(new Move(4, 5, Direction.North));
            snakeStub.SetHead(new Move(4, 4, Direction.North));

            return snakeStub;
        }

        protected static BattleField CreateBattleField(Guid idForOwnSnake)
        {
            var battleField = new BattleField(new Size(9, 9), 1);

            // Snake 0
            battleField[4, 4] = new FieldRow(FieldRowContent.Head, idForOwnSnake);
            battleField[4, 5] = new FieldRow(FieldRowContent.Body, idForOwnSnake);
            battleField[5, 5] = new FieldRow(FieldRowContent.Body, idForOwnSnake);
            battleField[5, 4] = new FieldRow(FieldRowContent.Tail, idForOwnSnake);

            // Snake 1
            var idForSnake1 = Guid.NewGuid();
            battleField[4, 3] = new FieldRow(FieldRowContent.Head, idForSnake1);
            battleField[4, 2] = new FieldRow(FieldRowContent.Body, idForSnake1);
            battleField[5, 2] = new FieldRow(FieldRowContent.Body, idForSnake1);
            battleField[5, 3] = new FieldRow(FieldRowContent.Tail, idForSnake1);

            // Snake 2
            var idForSnake2 = Guid.NewGuid();
            battleField[2, 2] = new FieldRow(FieldRowContent.Head, idForSnake2);
            battleField[3, 2] = new FieldRow(FieldRowContent.Body, idForSnake2);
            battleField[3, 3] = new FieldRow(FieldRowContent.Body, idForSnake2);
            battleField[2, 3] = new FieldRow(FieldRowContent.Body, idForSnake2);
            battleField[2, 4] = new FieldRow(FieldRowContent.Tail, idForSnake2);

            // Snake 3
            var idForSnake3 = Guid.NewGuid();
            battleField[2, 5] = new FieldRow(FieldRowContent.Head, idForSnake3);
            battleField[2, 6] = new FieldRow(FieldRowContent.Body, idForSnake3);
            battleField[2, 7] = new FieldRow(FieldRowContent.Tail, idForSnake3);

            return battleField;
        }

        protected static BrainChip CreateChipWithAndColoredHead(Guid snakeId)
        {
            var chipWithAndColoredHead = new BrainChip(new Size(5, 5), snakeId);

            // Own snake
            chipWithAndColoredHead.SetOwnHead(2, 2, AOColor.AndGrey, Direction.North);
            chipWithAndColoredHead.SetOwnBody(2, 3, Exclude.No, AOColor.OrGreen);
            chipWithAndColoredHead.SetOwnBody(3, 3, Exclude.No, AOColor.OrGreen);
            chipWithAndColoredHead.SetOwnTail(3, 2, Exclude.No, AOColor.OrGreen);

            // Snake 1
            chipWithAndColoredHead.SetEnemyHead(2, 1, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.SetEnemyBody(2, 0, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.SetEnemyBody(3, 0, Exclude.No, AOColor.AndBlack);
            chipWithAndColoredHead.SetEnemyTail(3, 1, Exclude.No, AOColor.AndBlack);

            // Snake 2
            chipWithAndColoredHead.SetEnemyHead(0, 0, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.SetEnemyBody(1, 0, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.SetEnemyBody(1, 1, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.SetEnemyBody(0, 1, Exclude.No, AOColor.AndGrey);
            chipWithAndColoredHead.SetEnemyTail(0, 2, Exclude.No, AOColor.AndGrey);

            // Snake 3
            chipWithAndColoredHead.SetEnemyHead(0, 3, Exclude.No, AOColor.OrBlue);
            chipWithAndColoredHead.SetEnemyBody(0, 4, Exclude.No, AOColor.OrBlue);

            return chipWithAndColoredHead;
        }
    }
}