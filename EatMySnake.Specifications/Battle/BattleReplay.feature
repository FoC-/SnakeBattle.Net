@scenarios
Feature: Battle replay
	In order to analize snake's behaviour or simply review a battle
	As a player
	I want to be able to see battle replay

@scenarios
Scenario: Step by step replay
	Given I have started battle replay
	And I have choosen step-by-step option
	When I press "Next step"
	Then I should be able to see next battle step
