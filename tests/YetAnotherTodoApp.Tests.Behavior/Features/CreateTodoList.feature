Feature: Create Todo List

Background: Sign in user
	Given a user with credentials:
		| email                       | password    |
		| admin@yetanothertodoapp.com | super$ecret |
	And credentials are valid

Scenario: Successfully created Todo List
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| title      | status_code |
		| Work stuff | 201         |

Scenario: Failed creating Todo List with title same as existing one
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| title | status_code |
		| Inbox | 400         |

Scenario: Failed creating Todo List with empty title
	When create a Todo List with <title>
	Then a server should return <status_code>

	Examples:
		| title | status_code |
		|       | 400         |