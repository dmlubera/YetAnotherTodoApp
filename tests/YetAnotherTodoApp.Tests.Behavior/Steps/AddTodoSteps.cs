using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using YetAnotherTodoApp.Tests.Behavior.Helpers;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    [Binding]
    [Scope(Feature = "Add Todo")]
    public class AddTodoSteps : AuthenticationBaseSteps
    {
        public AddTodoSteps(CustomWebApplicationFactory factory, ScenarioContext scenarioContext)
            : base(factory, scenarioContext)
        {
        }

        [When(@"add Todo with (.*), (.*)")]
        public async Task AddTodoWithoutSpecifiedTodoList(string title, string finishDate)
        {
            var request = new
            {
                Title = title,
                FinishDate = DateTime.Parse(finishDate)
            };

            _scenarioContext["Response"] =
                await _httpClient.PostAsync("api/todos", request.GetStringContent());
        }

        [When(@"add to (.*) Todo with (.*), (.*)")]
        public async Task AddTodoToSpecifiedTodoList(string todoList, string title, string finishDate)
        {
            var request = new
            {
                Title = title,
                FinishDate = DateTime.Parse(finishDate),
                TodoList = todoList
            };

            _scenarioContext["Response"] = 
                await _httpClient.PostAsync("api/todos", request.GetStringContent());
        }
    }
}
