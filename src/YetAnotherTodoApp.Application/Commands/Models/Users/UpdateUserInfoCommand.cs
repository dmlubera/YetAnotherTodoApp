using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Users
{
    public class UpdateUserInfoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        protected UpdateUserInfoCommand() { }

        public UpdateUserInfoCommand(Guid userId, string firstName, string lastName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}