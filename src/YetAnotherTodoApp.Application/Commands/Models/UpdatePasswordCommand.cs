using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdatePasswordCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
