using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Users
{
    public class UpdateEmailCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }

        protected UpdateEmailCommand() { }

        public UpdateEmailCommand(Guid userId, string email)
            => (UserId, Email) = (userId, email);
    }
}