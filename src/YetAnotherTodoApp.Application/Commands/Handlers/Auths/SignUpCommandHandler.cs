using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Extensions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Auths
{
    public class SignUpCommandHandler : ICommandHandler<SignUpCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IEncrypter _encrypter;
        private readonly ICache _cache;
        private readonly ILogger<SignUpCommandHandler> _logger;

        public SignUpCommandHandler(IUserRepository repository, IEncrypter encrypter,
            ICache cache, ILogger<SignUpCommandHandler> logger)
        {
            _repository = repository;
            _encrypter = encrypter;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(SignUpCommand command)
        {
            if (await _repository.CheckIfEmailIsInUseAsync(command.Email))
                throw new EmailInUseException(command.Email);
            if (await _repository.CheckIfUsernameIsInUseAsync(command.Username))
                throw new UsernameInUseException(command.Username);
            if (string.IsNullOrWhiteSpace(command.Password))
                throw new InvalidPasswordFormatException();

            var passwordSalt = _encrypter.GetSalt();
            var passwordHash = _encrypter.GetHash(command.Password, passwordSalt);
            var user = new User(command.Username, command.Email, passwordHash, passwordSalt);
            _cache.SetResourceIdentifier(command.CacheTokenId, user.Id);

            await _repository.AddAsync(user);

            _logger.LogTrace($"User with ID: {user.Id} has been created.");
        }
    }
}