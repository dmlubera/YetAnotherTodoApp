Feature: Sign in

Scenario: User with valid credentials is successfully logged in
	Given a user with <email> and <password>
	And credentials are valid
	Then a server should return <status_code>

	Examples:
		| email                       | password    | status_code |
		| admin@yetanothertodoapp.com | super$ecret | 200         |

Scenario: User with invalid credentials cannot log in
	Given a user with <email> and <password>
	And credentials are not valid
	Then a server should return <status_code>

	Examples:
		| email                          | password    | status_code |
		| nonadmin@yetanothertodoapp.com | super$ecret | 400         |