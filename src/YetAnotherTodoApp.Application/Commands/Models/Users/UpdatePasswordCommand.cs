using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Users
{
    public class UpdatePasswordCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }

        public UpdatePasswordCommand(Guid userId, string password)
            => (UserId, Password) = (userId, password);
    }
}