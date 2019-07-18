Feature: omicron
	In order to connect, disconnect, and change test parameters
	As a developer
	I want to be able to manage Omicron Test Set

@Omicron
Scenario: findCMC
	Given I start the application
	And I have Omicron Test Set available on network
	Then the result should be a DeviceID on the screen

Scenario: initialSetup
	Given I have a DeviceID
	When I have press connect
	Then the result should be a ok on the screen

Scenario: turnOnCMC
	Given I have a DeviceID
	And I have initialSetup is successfull
	Then the result should be Omicron Test Set to power up

Scenario: turnOffCMC
	Given I have a DeviceID
	And I have turn on Omicron Test Set
	When I press disconnect
	Then the result should be Omicron Test Set to power down
	
Scenario: releaseOmicron
	Given I have turn off Omicron Test Set
	Then the result should be a ok on screen
