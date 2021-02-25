using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateUserPasswordCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
