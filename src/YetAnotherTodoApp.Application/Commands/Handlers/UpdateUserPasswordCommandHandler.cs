using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateUserPasswordCommandHandler : ICommandHandler<UpdateUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncrypter _encrypter;

        public UpdateUserPasswordCommandHandler(IUserRepository userRepository, IEncrypter encrypter)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
        }

        public async Task HandleAsync(UpdateUserPasswordCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);
            user.UpdatePassword(passwordHash, passwordSalt);

            await _userRepository.SaveChangesAsync();
        }
    }
}
