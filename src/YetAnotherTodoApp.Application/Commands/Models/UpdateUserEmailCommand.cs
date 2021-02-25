using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateUserEmailCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
