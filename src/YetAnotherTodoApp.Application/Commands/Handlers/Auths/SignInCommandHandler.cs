using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Extensions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Auths
{
    public class SignInCommandHandler : ICommandHandler<SignInCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IEncrypter _encrypter;
        private readonly ICache _cache;
        private readonly ILogger<SignInCommandHandler> _logger;

        public SignInCommandHandler(IUserRepository repository, IJwtHelper jwtHelper,
            IEncrypter encrypter, ICache cache, ILogger<SignInCommandHandler> logger)
        {
            _repository = repository;
            _jwtHelper = jwtHelper;
            _encrypter = encrypter;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(SignInCommand command)
        {
            var user = await _repository.GetByEmailAsync(command.Email)
                ?? throw new InvalidCredentialsException();
            var hash = _encrypter.GetHash(command.Password, user.Password.Salt);
            if (user.Password.Hash != hash)
                throw new InvalidCredentialsException();

            var jwtToken = _jwtHelper.GenerateJwtToken(user.Id);
            _cache.SetJwtToken(command.CacheTokenId, jwtToken);

            _logger.LogTrace($"User with ID: {user.Id} has been successfuly signed in with JWT Token: {jwtToken.Token}");
        }
    }
}