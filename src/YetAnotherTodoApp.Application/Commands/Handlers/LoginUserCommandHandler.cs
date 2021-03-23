using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IEncrypter _encrypter;
        private readonly IMemoryCache _cache;

        public LoginUserCommandHandler(IUserRepository userRepository, IJwtHelper jwtHelper, IEncrypter encrypter, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _encrypter = encrypter;
            _cache = cache;
        }

        public async Task HandleAsync(LoginUserCommand command)
        {
            var user = await _userRepository.GetByEmailAsync(command.Email)
                ?? throw new Exception("Invalid credentials");
            var hash = _encrypter.GetHash(command.Password, user.Password.Salt);
            if (user.Password.Hash != hash)
                throw new Exception("Invalid credentials");

            var jwtToken = _jwtHelper.GenerateJwtToken(user.Id);
            _cache.Set(command.TokenId, jwtToken);
        }
    }
}
