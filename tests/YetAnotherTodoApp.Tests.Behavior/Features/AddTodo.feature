Feature: Add Todo

Scenario: Add Todo without specifing Todo List
	Given a user with <email> and <password>
	And credentials are valid
	When add Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | name          | finish_date | status_code |
		| admin@yetanothertodoapp.com | super$ecret | Keep learning | 2038-01-19  | 201         |

Scenario: Add Todo with specifing non-existing Todo List
	Given a user with <email> and <password>
	And credentials are valid
	When add to <todo_list> Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | name          | finish_date | todo_list   | status_code |
		| admin@yetanothertodoapp.com | super$ecret | Keep learning | 2038-01-19  | Study stuff | 201         |

Scenario: Add Todo with specifing existing Todo List
	Given a user with <email> and <password>
	And credentials are valid
	When add to <todo_list> Todo with <name>, <finish_date>
	Then a server should return <status_code>

	Examples:
		| email                       | password    | name          | finish_date | todo_list | status_code |
		| admin@yetanothertodoapp.com | super$ecret | Keep learning | 2038-01-19  | Inbox     | 201         |