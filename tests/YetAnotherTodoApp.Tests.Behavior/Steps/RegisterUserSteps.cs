using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using YetAnotherTodoApp.Tests.Behavior.Helpers;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    [Binding]
    public class RegisterUserSteps : CustomWebApplicationFactory
    {
        private readonly HttpClient _httpClient;
        private readonly ScenarioContext _scenarioContext;

        public RegisterUserSteps(CustomWebApplicationFactory factory, ScenarioContext scenarioContext)
        {
            _httpClient = factory.CreateClient();
            _scenarioContext = scenarioContext;
        }

        [Given(@"a new user with (.*) and (.*)")]
        public async Task RegisterUserWithGivenData(string email, string password)
        {
            var request = new
            {
                Username = "testusername",
                Email = email,
                Password = password
            };

            _scenarioContext["Response"] = 
                await _httpClient.PostAsync("api/auth/sign-up", request.GetStringContent());
        }
    }
}