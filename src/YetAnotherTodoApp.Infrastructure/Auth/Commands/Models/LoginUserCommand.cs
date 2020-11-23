using System;
using YetAnotherTodoApp.Application.Commands;

namespace YetAnotherTodoApp.Infrastructure.Auth.Commands.Models
{
    public class LoginUserCommand : ICommand
    {
        public Guid TokenId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}