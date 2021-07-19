using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models.TodoLists
{
    public class GetTodoListsQuery : IQuery<IEnumerable<TodoListDto>>
    {
        public Guid UserId { get; set; }

        public GetTodoListsQuery(Guid id)
            => UserId = id;
    }
}