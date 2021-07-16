using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Todos
{
    public class UpdateTodoPriorityRequestExample : IExamplesProvider<UpdateTodoPriorityRequest>
    {
        public UpdateTodoPriorityRequest GetExamples()
            => new UpdateTodoPriorityRequest
            {
                Priority = TodoPriority.High
            };
    }
}
