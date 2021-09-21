Feature: Register new user

Scenario: Register new user with valid data
Given a new user with <email> and <password> 
Then a server should return <status_code>

Examples: 
	| email                      | password    | status_code |
	| test@yetanothertodoapp.com | super$ecret | 201         |

Scenario: Register new user with invalid data
Given a new user with <email> and <password>
Then a server should return <status_code>

Examples: 
	| email                      | password    | status_code |
	| test@                      | super$ecret | 400         |
	| test@yetanothertodoapp.com | .           | 400         |

Scenario: Register new user with existing email
Given a new user with <email> and <password>
Then a server should return <status_code>

Examples: 
	| email                       | password    | status_code |
	| admin@yetanothertodoapp.com | super$ecret | 400         |