using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Models.Todos
{
    public class AddTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
        public TodoPriority? Priority { get; set; }
        public ICollection<StepDto> Steps { get; set; }
    }
}