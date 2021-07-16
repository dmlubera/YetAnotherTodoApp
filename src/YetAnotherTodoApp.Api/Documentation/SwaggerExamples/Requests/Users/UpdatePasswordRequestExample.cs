using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Users
{
    public class UpdatePasswordRequestExample : IExamplesProvider<UpdatePasswordRequest>
    {
        public UpdatePasswordRequest GetExamples()
            => new UpdatePasswordRequest
            {
                Password = "newSuperSecretPassword"
            };
    }
}