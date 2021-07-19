using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Todos
{
    public class UpdateTodoStatusExample : IExamplesProvider<UpdateTodoStatusRequest>
    {
        public UpdateTodoStatusRequest GetExamples()
            => new UpdateTodoStatusRequest
            {
                Status = TodoStatus.Done
            };
    }
}