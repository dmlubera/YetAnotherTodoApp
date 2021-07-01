using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Extensions;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/auth/")]
    public class AuthController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ICache _cache;

        public AuthController(ICommandDispatcher commandDispatcher, ICache cache)
        {
            _commandDispatcher = commandDispatcher;
            _cache = cache;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            var command = new SignUpCommand(request.Username.ToLower(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);
            var userId = _cache.GetId(command.CacheTokenId);

            return Created($"/api/users/{userId}", null);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            var command = new SignInCommand(request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return Ok(_cache.GetJwt(command.CacheTokenId));
        }
    }
}