using System;
using Machine.Specifications;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Snake.Implementation;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BrainChip))]
    class when_head_position_is_set
    {
        private Establish context = () =>
        {
            brainChip = new BrainChip(new Size(3, 3), Guid.NewGuid());
            currentHeadPosition = brainChip.GetOwnHead();

            newHeadPosition = new Move(0, 0, Direction.East);
        };

        private Because of = () =>
        {
            brainChip.SetOwnHead(newHeadPosition.X, newHeadPosition.Y, AOColor.AndGrey, newHeadPosition.direction);
        };

        private It should_remove_previous_head = () =>
        {
            brainChip[currentHeadPosition.X, currentHeadPosition.Y].ShouldBeNull();
        };

        private It should_change_head_position_to_new_head_position = () =>
        {
            brainChip.GetOwnHead().ShouldEqual(newHeadPosition);
        };

        private static BrainChip brainChip;
        private static Move newHeadPosition;
        private static Move currentHeadPosition;
    }
}