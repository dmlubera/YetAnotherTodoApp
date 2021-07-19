using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Auths
{
    public class SignInCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        protected SignInCommand() { }

        public SignInCommand(string email, string password)
        {
            Email = email;
            Password = password;
            CacheTokenId = Guid.NewGuid();
        }
    }
}