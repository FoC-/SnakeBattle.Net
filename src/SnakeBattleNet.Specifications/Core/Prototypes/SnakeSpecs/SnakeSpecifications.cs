using Machine.Specifications;
using NSubstitute;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Specifications.Tests.Core
{
    [Ignore("Restuta: please finish this test")]
    [Subject(typeof(BattleSnake))]
    public class when_moving_forward
    {
        Establish context = () =>
        {
            var mindStub = Substitute.For<IMind>();
            mindStub.GetMoveDirection(Arg.Any<VisibleArea>()).Returns(MoveDirection.Forward);

            battleSnake = new BattleSnake(new Snake(), mindStub);

            originalHeadCoordinates = battleSnake.Body.Head.Coordinates;
        };

        Because of = () =>
        {
            battleSnake.Move();
        };

        It should_change_its_body_position = () =>
        {
            originalHeadCoordinates.ShouldEqual(new Coordinates(originalHeadCoordinates.X, originalHeadCoordinates.Y - 1));
        };

        private static BattleSnake battleSnake;
        private static Coordinates originalHeadCoordinates;
    }
}