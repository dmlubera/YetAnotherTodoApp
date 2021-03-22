using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Models;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/auth/")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMemoryCache _cache;

        public AuthController(ICommandDispatcher commandDispatcher, IMemoryCache cache)
        {
            _commandDispatcher = commandDispatcher;
            _cache = cache;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Username.ToLower(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticateUserRequest request)
        {
            var command = new LoginUserCommand(Guid.NewGuid(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);
            return Ok(_cache.Get(command.TokenId));
        }
    }
}
