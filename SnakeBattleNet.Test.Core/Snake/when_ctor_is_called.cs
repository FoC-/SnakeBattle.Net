using System;
using Machine.Specifications;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake.Implementation;
using It = Machine.Specifications.It;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BrainChip))]
    class when_ctor_is_called
    {
        private Establish context = () =>
        {
            headInTheCenterOfTheChip = new Move(1, 1, Direction.North);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => brainChip = new BrainChip(new Size(3, 3), Guid.NewGuid()));
        };

        private It should_not_fail = () =>
        {
            exception.ShouldBeNull();
        };

        private It should_contain_head_in_the_center_of_the_chip = () =>
        {
            brainChip.GetOwnHead().ShouldEqual(headInTheCenterOfTheChip);
        };

        private static Exception exception;
        private static BrainChip brainChip;
        private static Move headInTheCenterOfTheChip;
    }
}