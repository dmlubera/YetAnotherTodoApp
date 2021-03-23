using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class SignUpCommandHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public SignUpCommandHandler(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public async Task HandleAsync(SignUpCommand command)
        {
            if (await _userRepository.CheckIfEmailIsInUseAsync(command.Email))
                throw new EmailInUseException(command.Email);
            if (await _userRepository.CheckIfUsernameIsInUseAsync(command.Username))
                throw new UsernameInUseException(command.Username);

            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);
            var user = new User(command.Username, command.Email, passwordHash, passwordSalt);

            await _userRepository.AddAsync(user);
        }
    }
}