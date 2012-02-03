Feature: Battle progrees
	In order to have fun
	As a player
	I want to be able to see battle

Scenario: Snake battle
	Given I have battle field
	And I have 4 loaded snakes
	When I press "Start Battle"
	Then the battlefield should change
