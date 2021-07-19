using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models.Todos
{
    public class GetTodoQuery : IQuery<TodoDto>
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }

        public GetTodoQuery(Guid userId, Guid todoId)
        {
            UserId = userId;
            TodoId = todoId;
        }
    }
}