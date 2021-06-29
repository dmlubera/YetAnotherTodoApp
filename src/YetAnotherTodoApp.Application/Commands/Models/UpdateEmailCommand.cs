using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateEmailCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
