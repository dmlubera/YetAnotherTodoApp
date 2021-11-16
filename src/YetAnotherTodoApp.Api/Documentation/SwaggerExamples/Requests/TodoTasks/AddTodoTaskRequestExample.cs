using Swashbuckle.AspNetCore.Filters;
using System;
using YetAnotherTodoApp.Api.Models.TodoTasks;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.TodoTasks
{
    public class AddTodoTaskRequestExample : IExamplesProvider<AddTodoTaskRequest>
    {
        public AddTodoTaskRequest GetExamples()
            => new AddTodoTaskRequest
            {
                TodoId = Guid.Empty,
                Title = "Something to do",
                Description = "Not important"
            };
    }
}