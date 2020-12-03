using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Models;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/users/")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMemoryCache _cache;

        public UserController(ICommandDispatcher commandDispatcher, IMemoryCache cache)
        {
            _commandDispatcher = commandDispatcher;
            _cache = cache;
        }

        [HttpPost]
        public async Task RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };
            await _commandDispatcher.DispatchAsync(command);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> LoginUserAsync([FromBody] AuthenticateUserRequest request)
        {
            var command = new LoginUserCommand
            {
                TokenId = Guid.NewGuid(),
                Email = request.Email,
                Password = request.Password
            };
            await _commandDispatcher.DispatchAsync(command);
            return Ok(_cache.Get(command.TokenId));
        }

    }
}