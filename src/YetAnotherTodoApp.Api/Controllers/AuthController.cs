using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;

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

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            var command = new SignUpCommand(request.Username.ToLower(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return StatusCode(201);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            var command = new SignInCommand(Guid.NewGuid(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);
            return Ok(_cache.Get(command.TokenId));
        }
    }
}
