using System;
using EatMySnake.Core.Prototypes;
using Machine.Specifications;
using NSubstitute;

namespace EatMySnake.Specifications.Tests.Core
{
    [Subject(typeof (Coordinates))]
    public class when_created_with__x__and__y__
    {
        Establish context = () =>
        {
            coordinates = new Coordinates(x: 1, y: 2);
        };

        It should_initialize__X__property_correspondingly = () =>
            coordinates.X.ShouldEqual(1);

        It should_initialize__Y__property_correspondingly = () =>
            coordinates.Y.ShouldEqual(2);

        private static Coordinates coordinates;
    }
}

