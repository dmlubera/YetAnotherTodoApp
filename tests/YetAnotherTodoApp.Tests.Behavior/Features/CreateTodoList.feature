Feature: Create Todo List

Scenario: Successfully created Todo List
	Given a user with <email> and <password>
	And credentials are valid
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | title      | status_code |
		| admin@yetanothertodoapp.com | super$ecret | Work stuff | 201         |

Scenario: Failed creating Todo List with title same as existing one
	Given a user with <email> and <password>
	And credentials are valid
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | title | status_code |
		| admin@yetanothertodoapp.com | super$ecret | Inbox | 400         |

Scenario: Failed creating Todo List with empty title
	Given a user with <email> and <password>
	And credentials are valid
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | title | status_code |
		| admin@yetanothertodoapp.com | super$ecret |       | 400         |