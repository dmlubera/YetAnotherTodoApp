using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Users
{
    public class UpdateEmailRequestExample : IExamplesProvider<UpdateEmailRequest>
    {
        public UpdateEmailRequest GetExamples()
            => new UpdateEmailRequest
            {
                Email = "newjohnsdoeemail@yetanothertodoapp.com"
            };
    }
}