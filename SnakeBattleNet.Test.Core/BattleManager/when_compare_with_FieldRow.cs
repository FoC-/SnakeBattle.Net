using System;
using Machine.Specifications;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(ChipRow))]
    class when_compare_with_FieldRow
    {
        private It should_be_equal_for_null_and_undefined = () =>
        {
            FieldRow fieldRow = null;
            var chipRow = new ChipRow(AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_null_and_indefined = () =>
        {
            FieldRow fieldRow = null;
            var chipRow = new ChipRow(ChipRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_Empty = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Empty);
            var chipRow = new ChipRow(ChipRowContent.Empty, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_Empty_when_Except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Wall);
            var chipRow = new ChipRow(ChipRowContent.Empty, Exclude.Yes, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_Empty_and_indefined = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head);
            var chipRow = new ChipRow(ChipRowContent.Empty, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_Wall = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Wall);
            var chipRow = new ChipRow(ChipRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_Wall_when_Except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head);
            var chipRow = new ChipRow(ChipRowContent.Wall, Exclude.Yes, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_Wall_and_indefined = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head);
            var chipRow = new ChipRow(ChipRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_own_head = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_head_and_enemy = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();


            var fieldRow = new FieldRow(FieldRowContent.Head, enemySnakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_own_body = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_body_and_enemy = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, enemySnakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_own_body_and_enemy_and_except = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, enemySnakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_own_tail = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Tail, snakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnTail, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_tail_and_enemy = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Tail, enemySnakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnTail, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_own_tail_and_enemy_and_except = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Tail, enemySnakeId);
            var chipRow = new ChipRow(ChipRowContent.OwnTail, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_head = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyHead, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_head_and_own = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_head_and_own_and_except = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyHead, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_body = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_body_and_own = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_body_and_own_and_except = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_tail = () =>
        {
            var snakeId = Guid.NewGuid();
            var enemySnakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_tail_and_own = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_tail_and_own_and_except = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(ChipRowContent.EnemyBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_undefined = () =>
        {
            var snakeId = Guid.NewGuid();

            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ChipRow(AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };
    }
}