using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(ModuleRow))]
    class when_compare_with_FieldRow
    {
        private It should_be_equal_for_null_and_undefined = () =>
        {
            FieldRow fieldRow = null;
            var chipRow = new ModuleRow(AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_null_and_indefined = () =>
        {
            FieldRow fieldRow = null;
            var chipRow = new ModuleRow(ModuleRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_Empty = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Empty, "");
            var chipRow = new ModuleRow(ModuleRowContent.Empty, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_Empty_when_Except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Wall, "");
            var chipRow = new ModuleRow(ModuleRowContent.Empty, Exclude.Yes, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_Empty_and_indefined = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, "");
            var chipRow = new ModuleRow(ModuleRowContent.Empty, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_Wall = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Wall, "");
            var chipRow = new ModuleRow(ModuleRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_Wall_when_Except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, "");
            var chipRow = new ModuleRow(ModuleRowContent.Wall, Exclude.Yes, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_Wall_and_indefined = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, "");
            var chipRow = new ModuleRow(ModuleRowContent.Wall, Exclude.No, AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_own_head = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_head_and_enemy = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, enemySnakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };


        private It should_be_equal_for_own_body = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_body_and_enemy = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, enemySnakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_own_body_and_enemy_and_except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, enemySnakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_own_tail = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Tail, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnTail, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_own_tail_and_enemy = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Tail, enemySnakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnTail, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_own_tail_and_enemy_and_except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Tail, enemySnakeId);
            var chipRow = new ModuleRow(ModuleRowContent.OwnTail, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_head = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_head_and_own = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_head_and_own_and_except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Head, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyHead, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_body = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_body_and_own = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_body_and_own_and_except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };


        private It should_be_equal_for_enemy_tail = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, enemySnakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_not_be_equal_for_enemy_tail_and_own = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.No, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeFalse();
        };

        private It should_be_equal_for_enemy_tail_and_own_and_except = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(ModuleRowContent.EnemyBody, Exclude.Yes, AOColor.AndGrey, snakeId);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private It should_be_equal_for_undefined = () =>
        {
            var fieldRow = new FieldRow(FieldRowContent.Body, snakeId);
            var chipRow = new ModuleRow(AOColor.AndGrey);

            chipRow.Equals(fieldRow).ShouldBeTrue();
        };

        private static string snakeId = "Own-snake-Id";
        private static string enemySnakeId = "Enemy-snake-ID";
    }
}