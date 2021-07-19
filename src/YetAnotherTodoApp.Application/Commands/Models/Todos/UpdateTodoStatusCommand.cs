using System;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.Commands.Models.Todos
{
    public class UpdateTodoStatusCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public TodoStatus Status { get; set; }

        public UpdateTodoStatusCommand(Guid userId, Guid todoId, TodoStatus status)
        {
            UserId = userId;
            TodoId = todoId;
            Status = status;
        }
    }
}
