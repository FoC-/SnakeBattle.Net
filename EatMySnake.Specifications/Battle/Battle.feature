@scenarios
	Feature: Battle
	As a snake owner
	I want to be able to run a battle with my snake involved
	So I will be able to see how it performs comparing to other snakes

@scenarios
Scenario: Battle that ends by timeout
	Given I have started a battle
	And it cannot be finished during pre-determined amount of turns 
		#(550 in the original game)
	Then it should be finished automatically after the last turn

Scenario: Battle that ends when one snake is left
	Given I have started a battle with more than one snake
	And only one snake is left after some time
	Then battle should be finished
		And snake should be anounced as a winner
