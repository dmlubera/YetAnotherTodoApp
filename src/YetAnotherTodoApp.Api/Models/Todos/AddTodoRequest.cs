using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Api.Models.Todos
{
    public class AddTodoRequest
    {
        public string Title { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
        public ICollection<StepDto> Steps { get; set; }
    }
}