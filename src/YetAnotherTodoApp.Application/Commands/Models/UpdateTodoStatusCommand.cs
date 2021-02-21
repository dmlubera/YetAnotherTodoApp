using System;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateTodoStatusCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public TodoStatus Status { get; set; }
    }
}
