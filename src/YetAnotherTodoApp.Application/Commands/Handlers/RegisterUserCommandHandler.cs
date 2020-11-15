using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repostiories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public RegisterUserCommandHandler(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public async Task HandleAsync(RegisterUserCommand command)
        {
            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);
            var user = new User(command.Username, command.Email, passwordHash, passwordSalt);

            await _userRepository.AddAsync(user);
        }
    }
}