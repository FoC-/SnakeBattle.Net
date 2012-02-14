@scenarios
	Feature: Battle
	As a snake owner
	I want to be able to run a battle with my snake involved
	So I will be able to see how it performs comparing to other snakes

Scenario: Battle that ends by timeout
	Given I have started a battle
	When it wasn't finished after maximum amount of rounds 
		#(550 in the original game)
	Then it should be finished automatically after the last round

Scenario: Battle that ends when one snake is left
	Given I have started a battle with more than one snake
	When only one snake is left after some time
	Then battle should be finished
		And snake should be anounced as a winner

Scenario: All snakes are stuck
	Given I have started a battle
	When all snakes are stuck
	Then battle should be finished automatically

Scenario: No snake can be bitten
	Given I have started a battle
	When all snakes except one are stuck in the way their tail can't be bitten
		And one snake is not stuck
	Then battle should be finished automatically

Scenario: Only one snake is left
	Given I have started a battle
	When several turns passed
		And only one snake is left
	Then battle should be finished

Scenario: Battle is finished
	Given I have started a battle
	When it was finished by any reason
	Then all snakes should recive score points according to their body lenth at the moment of the end of the battle
