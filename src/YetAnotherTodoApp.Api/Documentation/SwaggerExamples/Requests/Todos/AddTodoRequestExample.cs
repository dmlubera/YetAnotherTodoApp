using Swashbuckle.AspNetCore.Filters;
using System;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Todos
{
    public class AddTodoRequestExample : IExamplesProvider<AddTodoRequest>
    {
        public AddTodoRequest GetExamples()
            => new AddTodoRequest
            {
                Title = "Something not really important",
                FinishDate = DateTime.UtcNow.AddDays(7).Date,
            };
    }
}