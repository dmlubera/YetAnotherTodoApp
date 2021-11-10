using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.TodoTasks;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.TodoTasks
{
    public class UpdateTodoTaskRequestExample : IExamplesProvider<UpdateTodoTaskRequest>
    {
        public UpdateTodoTaskRequest GetExamples()
            => new UpdateTodoTaskRequest
            {
                Title = "Task in todo",
                Description = "First part of todo"
            };
    }
}