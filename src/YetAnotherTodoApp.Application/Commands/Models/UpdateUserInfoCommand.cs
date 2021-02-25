using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateUserInfoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
