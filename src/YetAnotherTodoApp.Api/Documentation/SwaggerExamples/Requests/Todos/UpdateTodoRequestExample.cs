using Swashbuckle.AspNetCore.Filters;
using System;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Todos
{
    public class UpdateTodoRequestExample : IExamplesProvider<UpdateTodoRequest>
    {
        public UpdateTodoRequest GetExamples()
            => new UpdateTodoRequest
            {
                Title = "Something really important",
                Description = "The title has been updated",
                FinishDate = DateTime.UtcNow.AddDays(1).Date
            };
    }
}