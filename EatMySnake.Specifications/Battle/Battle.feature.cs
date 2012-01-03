﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.239
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EatMySnake.Specifications.Battle
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Battle")]
    [NUnit.Framework.CategoryAttribute("scenarios")]
    public partial class BattleFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Battle.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Battle", "As a snake owner\r\nI want to be able to run a battle with my snake involved\r\nSo I " +
                    "will be able to see how it performs comparing to other snakes", ProgrammingLanguage.CSharp, new string[] {
                        "scenarios"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Battle that ends by timeout")]
        [NUnit.Framework.CategoryAttribute("scenarios")]
        public virtual void BattleThatEndsByTimeout()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Battle that ends by timeout", new string[] {
                        "scenarios"});
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given("I have started a battle");
#line 10
 testRunner.And("it cannot be finished during pre-determined amount of turns");
#line 12
 testRunner.Then("it should be finished automatically after the last turn");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Battle that ends when one snake is left")]
        public virtual void BattleThatEndsWhenOneSnakeIsLeft()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Battle that ends when one snake is left", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 15
 testRunner.Given("I have started a battle with more than one snake");
#line 16
 testRunner.And("only one snake is left after some time");
#line 17
 testRunner.Then("battle should be finished");
#line 18
  testRunner.And("snake should be anounced as a winner");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("All snakes are stuck")]
        public virtual void AllSnakesAreStuck()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("All snakes are stuck", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given("I have started a battle");
#line 22
 testRunner.And("all snakes are stuck");
#line 23
 testRunner.Then("battle should be finished automatically");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("No snake can be bitten")]
        public virtual void NoSnakeCanBeBitten()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No snake can be bitten", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line 26
 testRunner.Given("I have started a battle");
#line 27
 testRunner.And("all snakes except one are stuck in the way their tail can\'t be bitten");
#line 28
 testRunner.And("one snake is not stuck");
#line 29
 testRunner.Then("battle should be finished automatically");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Only one snake is left")]
        public virtual void OnlyOneSnakeIsLeft()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Only one snake is left", ((string[])(null)));
#line 31
this.ScenarioSetup(scenarioInfo);
#line 32
 testRunner.Given("I have started a battle");
#line 33
 testRunner.And("after several turns only one snake is left");
#line 34
 testRunner.Then("battle should be finished");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
