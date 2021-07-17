using Swashbuckle.AspNetCore.Filters;
using System;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Todos
{
    public class AddTodoRequestExample : IExamplesProvider<AddTodoRequest>
    {
        public AddTodoRequest GetExamples()
            => new AddTodoRequest
            {
                Title = "Something not really important",
                Project = "Not important stuff",
                FinishDate = DateTime.UtcNow.AddDays(7).Date,
                Priority = TodoPriority.Low
            };
    }
}