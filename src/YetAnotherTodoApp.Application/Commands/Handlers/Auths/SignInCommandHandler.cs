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
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IEncrypter _encrypter;
        private readonly ICache _cache;

        public SignInCommandHandler(IUserRepository userRepository, IJwtHelper jwtHelper, IEncrypter encrypter, ICache cache)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _encrypter = encrypter;
            _cache = cache;
        }

        public async Task HandleAsync(SignInCommand command)
        {
            var user = await _userRepository.GetByEmailAsync(command.Email)
                ?? throw new InvalidCredentialsException();
            var hash = _encrypter.GetHash(command.Password, user.Password.Salt);
            if (user.Password.Hash != hash)
                throw new InvalidCredentialsException();

            var jwtToken = _jwtHelper.GenerateJwtToken(user.Id);
            _cache.SetJwtToken(command.CacheTokenId, jwtToken);
        }
    }
}