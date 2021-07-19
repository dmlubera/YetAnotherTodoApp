using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Auths;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Auths
{
    public class SignUpRequestExample : IExamplesProvider<SignUpRequest>
    {
        public SignUpRequest GetExamples()
            => new SignUpRequest
            {
                Username = "johndoe",
                Email = "johndoe@yetanothertodoapp.com",
                Password = "superSecretPassword"
            };
    }
}