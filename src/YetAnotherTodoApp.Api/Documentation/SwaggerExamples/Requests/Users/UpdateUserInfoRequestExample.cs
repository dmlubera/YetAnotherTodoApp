using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Users
{
    public class UpdateUserInfoRequestExample : IExamplesProvider<UpdateUserInfoRequest>
    {
        public UpdateUserInfoRequest GetExamples()
            => new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };
    }
}