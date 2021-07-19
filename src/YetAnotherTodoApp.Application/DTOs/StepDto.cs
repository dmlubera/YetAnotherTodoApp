using System;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class StepDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsFinished { get; set; }
    }
}