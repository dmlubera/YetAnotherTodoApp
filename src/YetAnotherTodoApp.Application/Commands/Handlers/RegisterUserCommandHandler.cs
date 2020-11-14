using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repostiories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(RegisterUserCommand command)
        {
            var user = new User(command.Username, command.Email, command.Password, "salt");
            await _userRepository.AddAsync(user);
        }
    }
}