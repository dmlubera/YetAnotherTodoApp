using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class SignInCommand : ICommand
    {
        public Guid TokenId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        protected SignInCommand() { }

        public SignInCommand(Guid tokenId, string email, string password)
        {
            TokenId = tokenId;
            Email = email;
            Password = password;
        }
    }
}
