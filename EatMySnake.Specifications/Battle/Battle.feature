@scenarios
	Feature: Battle
	As a snake owner
	I want to be able to run a battle with my snake involved
	So I will be able to see how it performs comparing to other snakes

@scenarios
Scenario: Battle that ends by timeout
	Given I have started a battle
	And it cannot be finished during pre-determined amount of moves
	Then it should be finished automatically
