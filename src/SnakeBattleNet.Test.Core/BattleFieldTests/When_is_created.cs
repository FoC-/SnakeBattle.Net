﻿using System.Linq;
using Machine.Specifications;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Test.Core.BattleFieldTests
{
    [Subject(typeof(BattleField))]
    class When_is_created
    {
        Establish context = () =>
        {
            battleField = new BattleField();
        };

        It should_exact_number_of_cells = () =>
            battleField.Count().ShouldEqual(BattleField.SideLength * BattleField.SideLength);

        It should_exact_number_of_wals = () =>
            battleField.Count(c => c.Value == Content.Wall).ShouldEqual(BattleField.SideLength * 4 - 4);

        It should_exact_number_of_empty = () =>
            battleField.Count(c => c.Value == Content.Empty).ShouldEqual((BattleField.SideLength * BattleField.SideLength) - (BattleField.SideLength * 4 - 4));

        It should_exact_number_of_gateways = () =>
            battleField.Gateways.Count.ShouldEqual(4);

        private static BattleField battleField;
    }
}