using Machine.Specifications;
using SnakeBattleNet.Core.Prototypes;

namespace SnakeBattleNet.Specifications.Core.Prototypes.BattleSpecs
{
    [Subject(typeof (Battle))]
    public class when_created_without_arguments
    {
        Because of = () =>
            battle = new Battle();

        It should_have__MaxRounds__set_to_default_value_which_is__500__ = () =>
            battle.MaxRounds.ShouldEqual(550);

        private static Battle battle;
    }

    [Subject(typeof(Battle))]
    public class when_created_with_specified__fieldSize__and__maxRounds__
    {
        Because of = () =>
            battle = new Battle(fieldSize: Specifiedvalue, maxRounds: Specifiedvalue);

        It should_have__FieldSize__set_to_specified_value = () =>
        {
            battle.FieldSize.ShouldEqual(Specifiedvalue);
        };

        It should_have__MaxSize__set_to_specified_value = () =>
        {
            battle.MaxRounds.ShouldEqual(Specifiedvalue);
        };

        private static Battle battle;
        private const int Specifiedvalue = 10;
    }

    //[Subject(typeof(Battle), "PlayToEnd()")]
    //public class when_called
    //{
    //    It should_create__BattleReplay__with__events_that_represents_game_hystory = () =>
    //    {   
            
    //    };
    //}
}

