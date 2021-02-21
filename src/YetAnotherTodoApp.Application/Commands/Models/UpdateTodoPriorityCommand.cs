using System;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateTodoPriorityCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public TodoPriority Priority { get; set; }
    }
}
