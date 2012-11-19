using System;
using Machine.Specifications;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake.Implementation;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BrainModule))]
    class when_ctor_is_called
    {
        private Establish context = () =>
        {
            headInTheCenterOfTheChip = new Move(1, 1, Direction.North);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => _brainModule = new BrainModule("Module-Id", new Size(3, 3), "Snake-Id"));
        };

        private It should_not_fail = () =>
        {
            exception.ShouldBeNull();
        };

        private It should_contain_head_in_the_center_of_the_chip = () =>
        {
            _brainModule.GetOwnHead().ShouldEqual(headInTheCenterOfTheChip);
        };

        private static Exception exception;
        private static BrainModule _brainModule;
        private static Move headInTheCenterOfTheChip;
    }
}