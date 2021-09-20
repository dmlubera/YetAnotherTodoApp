Feature: Register new user

Scenario: Register new user with valid data
Given a new user with <email> and <password> 
Then a server should return <status_code>

Examples: 
	| email                      | password    | status_code |
	| test@yetanothertodoapp.com | super$ecret | 201         |