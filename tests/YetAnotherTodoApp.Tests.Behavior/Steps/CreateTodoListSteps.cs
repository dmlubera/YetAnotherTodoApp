using System.Threading.Tasks;
using TechTalk.SpecFlow;
using YetAnotherTodoApp.Tests.Behavior.Helpers;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    [Binding]
    [Scope(Feature = "Create Todo List")]
    public class CreateTodoListSteps : AuthenticationBaseSteps
    {
        public CreateTodoListSteps(CustomWebApplicationFactory factory, ScenarioContext scenarioContext)
            : base(factory, scenarioContext)
        {
        }

        [When(@"create a Todo List with (.*)")]
        public async Task CreateTodoListWithGivenTitle(string title)
        {
            var request = new
            {
                Title = title
            };

            _scenarioContext["Response"] = await _httpClient.PostAsync("api/todolist/", request.GetStringContent());
        }
    }
}