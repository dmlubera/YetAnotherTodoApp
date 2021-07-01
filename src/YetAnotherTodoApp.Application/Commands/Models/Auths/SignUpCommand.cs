using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Auths
{
    public class SignUpCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public SignUpCommand(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            CacheTokenId = Guid.NewGuid();
        }
    }
}