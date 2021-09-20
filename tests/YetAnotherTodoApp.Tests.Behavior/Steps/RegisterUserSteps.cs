using FluentAssertions;
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
        private HttpResponseMessage _httpResponse;

        public RegisterUserSteps(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
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

            _httpResponse = 
                await _httpClient.PostAsync("api/auth/sign-up", request.GetStringContent());
        }

        [Then(@"a server should return (.*)")]
        public void TheStatusCodeShouldBe(int statusCode)
        {
            ((int)_httpResponse.StatusCode).Should().Be(statusCode);
        }
    }
}