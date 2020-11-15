namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class RegisterUserCommand : ICommand
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}