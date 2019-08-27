Feature: communication
	In order to connect and disconnect the clients
	As a developer
	I want to be able to manage connections

@TCPModbus
Scenario: Connect to the client
	Given I have entered a connectible IpAddress
	And I have entered connectible modbus port
	When I press connect
	Then the result should be connected

Scenario: Disconnect from the client
	Given I have connection to the client
	When I press disconnect
	Then the result should be !connected

Scenario: Cannot connect to the client with wrong IpAddress
	Given I have entered a non-connectible IpAddress
	And I have entered connectible modbus port
	When I press connect
	Then the result should be a TimeOut

Scenario: Cannot connect to the client with wrong modbus port
	Given I have entered a connectible IpAddress
	And I have entered non-connectible modbus port
	When I press connect
	Then the result should be a TimeOut

Scenario: Retrieve values from the client
	Given I have entered a connectible IpAddress
	And I have entered connectible modbus port
	When I press connect
	Then the result should be some numbers

