using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Application.DTOs;
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
                Priority = TodoPriority.Low,
                Steps = new List<StepDto>
                {
                    new StepDto
                    {
                        Title = "Not important step"
                    },
                    new StepDto
                    {
                        Title = "Really important step",
                        Description ="Should hurry up with this"
                    }
                }
            };
    }
}