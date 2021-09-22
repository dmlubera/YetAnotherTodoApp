using TechTalk.SpecFlow;

namespace YetAnotherTodoApp.Tests.Behavior.Steps
{
    [Binding]
    [Scope(Feature = "Sign in")]
    public class SignInSteps : AuthenticationBaseSteps
    {
        public SignInSteps(CustomWebApplicationFactory factory, ScenarioContext scenarioContext)
            : base(factory, scenarioContext)
        {
        }
    }
}