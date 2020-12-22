using System;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
