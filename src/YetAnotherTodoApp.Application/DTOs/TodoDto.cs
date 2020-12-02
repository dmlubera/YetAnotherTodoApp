using System;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
