using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UpdatePasswordCommandHandler> _logger;

        public UpdatePasswordCommandHandler(IUserRepository repository, IEncrypter encrypter, ILogger<UpdatePasswordCommandHandler> logger)
        {
            _repository = repository;
            _encrypter = encrypter;
            _logger = logger;
        }

        public async Task HandleAsync(UpdatePasswordCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            if (user is null)
                throw new UserNotExistException(command.UserId);

            var passwordHashWithUserSalt = _encrypter.GetHash(command.Password, user.Password.Salt);

            if (passwordHashWithUserSalt == user.Password.Hash)
                throw new UpdatePasswordToAlreadyUsedValueException();

            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);

            user.UpdatePassword(passwordHash, passwordSalt);

            await _repository.UpdateAsync(user);

            _logger.LogTrace($"Password of user with ID: {user.Id} has been updated.");
        }
    }
}