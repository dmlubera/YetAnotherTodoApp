using System;
using System.Collections.Generic;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class DetailedTodoListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<TodoDto> Todos { get; set; }
    }
}