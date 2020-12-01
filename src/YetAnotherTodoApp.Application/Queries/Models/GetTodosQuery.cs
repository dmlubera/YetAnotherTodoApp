using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Queries.Models
{
    public class GetTodosQuery : IQuery<IEnumerable<Todo>>
    {
        public Guid UserId { get; set; }
    }
}
