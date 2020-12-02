using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models
{
    public class GetTodosQuery : IQuery<IEnumerable<TodoDto>>
    {
        public Guid UserId { get; set; }
    }
}
