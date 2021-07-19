using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models.TodoLists
{
    public class GetTodoListQuery : IQuery<TodoListDto>
    {
        public Guid UserId { get; set; }
        public Guid TodoListId { get; set; }

        public GetTodoListQuery(Guid userId, Guid todoListId)
        {
            UserId = userId;
            TodoListId = todoListId;
        }
    }
}