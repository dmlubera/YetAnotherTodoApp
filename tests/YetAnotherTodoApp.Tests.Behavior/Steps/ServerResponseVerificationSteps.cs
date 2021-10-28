using FluentAssertions;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    [Binding]
    public class ServerResponseVerificationSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public ServerResponseVerificationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Then(@"a server should return (.*)")]
        public void TheStatusCodeShouldBe(int statusCode)
        {
            var httpResponse = _scenarioContext["Response"] as HttpResponseMessage;
            ((int)httpResponse.StatusCode).Should().Be(statusCode);
        }
    }
}