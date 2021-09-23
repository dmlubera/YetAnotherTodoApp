Feature: Add Todo

Background: Sign in user
	Given a user with credentials:
		| email                       | password    |
		| admin@yetanothertodoapp.com | super$ecret |
	And credentials are valid

Scenario: Add Todo without specifing Todo List
	When add Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| name          | finish_date | status_code |
		| Keep learning | 2038-01-19  | 201         |

Scenario: Add Todo with specifing non-existing Todo List
	When add to <todo_list> Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| name          | finish_date | todo_list   | status_code |
		| Keep learning | 2038-01-19  | Study stuff | 201         |

Scenario: Add Todo with specifing existing Todo List
	When add to <todo_list> Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| name          | finish_date | todo_list | status_code |
		| Keep learning | 2038-01-19  | Inbox     | 201         |