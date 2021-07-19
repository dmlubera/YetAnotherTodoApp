using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Auths;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Auths
{
    public class SignInRequestExample : IExamplesProvider<SignInRequest>
    {
        public SignInRequest GetExamples()
            => new SignInRequest
            {
                Email = "johndoe@yetanothertodoapp.com",
                Password = "superSecretPassword"
            };
    }
}