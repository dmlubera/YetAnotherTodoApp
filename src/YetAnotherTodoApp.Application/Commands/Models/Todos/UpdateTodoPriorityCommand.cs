using System;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.Commands.Models.Todos
{
    public class UpdateTodoPriorityCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public TodoPriority Priority { get; set; }

        public UpdateTodoPriorityCommand(Guid userId, Guid todoId, TodoPriority priority)
        {
            UserId = userId;
            TodoId = todoId;
            Priority = priority;
        }
    }
}
