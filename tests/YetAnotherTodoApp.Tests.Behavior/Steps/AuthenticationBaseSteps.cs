using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using YetAnotherTodoApp.Tests.Behavior.Helpers;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    public class AuthenticationBaseSteps : CustomWebApplicationFactory
    {
        protected readonly HttpClient _httpClient;
        protected readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _authResponse;

        public AuthenticationBaseSteps(CustomWebApplicationFactory factory,
            ScenarioContext scenarioContext)
        {
            _httpClient = factory.CreateClient();
            _scenarioContext = scenarioContext;
        }

        [Given("a user with (.*) and (.*)")]
        public async Task SignInWithGivenCredentials(string email, string password)
        {
            var request = new
            {
                Email = email,
                Password = password
            };

            _authResponse = await _httpClient.PostAsync("api/auth/sign-in", request.GetStringContent());
            if(_authResponse.IsSuccessStatusCode)
            {
                var jwtToken = JsonConvert.DeserializeObject<AuthSuccessfulResponse>(await _authResponse.Content.ReadAsStringAsync()).Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtToken);
            }
            _scenarioContext["Response"] = _authResponse;
        }

        [Given(@"credentials are valid")]
        public void SuccessfulAuthentication()
        {
            _authResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Given(@"credentials are not valid")]
        public void FailedAuthentication()
        {
            _authResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Given(@"a user with credentials:")]
        public async Task SignInWithGivenCredentials(Table table)
        {
            var credentials = table.CreateInstance<Credentials>();

            _authResponse = await _httpClient.PostAsync("api/auth/sign-in", credentials.GetStringContent());
            if (_authResponse.IsSuccessStatusCode)
            {
                var jwtToken = JsonConvert.DeserializeObject<AuthSuccessfulResponse>(await _authResponse.Content.ReadAsStringAsync()).Token;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtToken);
            }
            _scenarioContext["Response"] = _authResponse;
        }
    }
}