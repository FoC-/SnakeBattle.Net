using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core
{
    [Subject(typeof(BrainModule))]
    class when_head_position_is_set
    {
        private Establish context = () =>
        {
            _brainModule = new BrainModule(new Size(3, 3), "Snake-Id");
            currentHeadPosition = _brainModule.GetOwnHead();

            newHeadPosition = new Move(0, 0, Direction.East);
        };

        private Because of = () =>
        {
            _brainModule.SetOwnHead(newHeadPosition.X, newHeadPosition.Y, AOColor.AndGrey, newHeadPosition.Direction);
        };

        private It should_remove_previous_head = () =>
        {
            _brainModule[currentHeadPosition.X, currentHeadPosition.Y].ShouldBeNull();
        };

        private It should_change_head_position_to_new_head_position = () =>
        {
            _brainModule.GetOwnHead().ShouldEqual(newHeadPosition);
        };

        private static BrainModule _brainModule;
        private static Move newHeadPosition;
        private static Move currentHeadPosition;
    }
}