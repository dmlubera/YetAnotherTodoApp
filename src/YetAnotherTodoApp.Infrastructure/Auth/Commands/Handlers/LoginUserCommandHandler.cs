using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Repostiories;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Models;
using YetAnotherTodoApp.Infrastructure.Auth.Helpers;

namespace YetAnotherTodoApp.Infrastructure.Auth.Commands.Handlers
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
            var user = await _userRepository.GetByEmailAsync(command.Email);
            var hash = _encrypter.GetHash(command.Password, user.Salt);
            if(!string.Equals(hash, user.Password))
                throw new Exception("Invalid credentials");
                
            var jwtToken = _jwtHelper.GenerateJwtToken(user.Id);
            _cache.Set(command.TokenId, jwtToken);
        }
    }
}