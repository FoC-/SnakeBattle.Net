using Machine.Specifications;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Specifications.Core.Prototypes.CoordinatesSpecs
{
    [Subject(typeof (Coordinates))]
    public class when_created_with__x__and__y__
    {
        Because of = () =>
            coordinates = new Coordinates(x: 1, y: 2);

        It should_initialize__X__property_correspondingly = () =>
            coordinates.X.ShouldEqual(1);

        It should_initialize__Y__property_correspondingly = () =>
            coordinates.Y.ShouldEqual(2);

        private static Coordinates coordinates;
    }
}

