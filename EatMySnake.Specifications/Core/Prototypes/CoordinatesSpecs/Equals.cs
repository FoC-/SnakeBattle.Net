using System;
using Machine.Specifications;
using NSubstitute;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Specifications.Core.Prototypes.CoordinatesSpecs
{
    [Subject(typeof (Coordinates), "Equals")]
    public class when_called_on_self
    {
        Establish context = () =>
        {
            coordinates = new Coordinates(0, 0);
        };

        Because of = () =>
        {
            @equals = coordinates.Equals(coordinates);
        };

        It should_return__true__ = () =>
        {
            @equals.ShouldBeTrue();
        };

        private static Coordinates coordinates;
        private static bool @equals;
    }

    [Subject(typeof(Coordinates), "Equals")]
    public class when_called_on_object__a__to_compare_with__b__
    {
        private Establish context = () =>
        {
            a = new Coordinates(0, 0);
            b = new Coordinates(1, 1);
        };

        private Because of = () =>
        {
            resultA = a.Equals(b);
            resultB = b.Equals(a);
        };

        //a.Equals(b) returns the same value as b.Equals(a). 
        It should_return_the_same_value_as_if_it_was_called_on_object__b__to_compare_with__a__ = () =>
        {
            resultA.ShouldEqual(resultB);
        };

        private static Coordinates a;
        private static Coordinates b;
        private static bool resultA;
        private static bool resultB;
    }

    [Subject(typeof(Coordinates), "Equals")]
    public class when_called_on_object__a__to_compare_with__b__and_objects_are_different
    {
        private Establish context = () =>
        {
            a = new Coordinates(0, 0);
            b = new Coordinates(1, 1);
        };

        private Because of = () =>
        {
            result = a.Equals(b);
        };

        It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private static Coordinates a;
        private static Coordinates b;
        private static bool result;
    }

    [Subject(typeof(Coordinates), "Equals")]
    public class when_called_on_object__a__to_compare_with__b__and_objects_are_equal
    {
        private Establish context = () =>
        {
            a = new Coordinates(1, 2);
            b = new Coordinates(1, 2);
        };

        private Because of = () =>
        {
            result = a.Equals(b);
        };

        It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private static Coordinates a;
        private static Coordinates b;
        private static bool result;
    }

    [Subject(typeof (Coordinates), "Equals")]
    public class when_called_on_object__a__to_compare_with__null__
    {
        Because of = () =>
        {
            result = new Coordinates(0, 0).Equals(null);
        };

        It should_reutrn_false = () =>
        {
            result.ShouldBeFalse();
        };

        private static bool result;
    }
}

