using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Users
{
    public class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IEncrypter _encrypter;

        public UpdatePasswordCommandHandler(IUserRepository repository, IEncrypter encrypter)
            => (_repository, _encrypter) = (repository, encrypter);

        public async Task HandleAsync(UpdatePasswordCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            var passwordHashWithUserSalt = _encrypter.GetHash(command.Password, user.Password.Salt);

            if (passwordHashWithUserSalt == user.Password.Hash)
                throw new UpdatePasswordToAlreadyUsedValueException();

            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);

            user.UpdatePassword(passwordHash, passwordSalt);

            await _repository.SaveChangesAsync();
        }
    }
}