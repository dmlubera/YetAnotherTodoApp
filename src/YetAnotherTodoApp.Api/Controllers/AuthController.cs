using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models.Auths;
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

        /// <summary>
        /// Registers the user
        /// </summary>
        /// <response code="201">The user has been successfully registered</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("sign-up")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            var command = new SignUpCommand(request.Username.ToLower(), request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);
            var userId = _cache.GetResourceIdentifier(command.CacheTokenId);

            return Created($"/api/users/{userId}", null);
        }

        /// <summary>
        /// Logs the user in and gets the JWT token 
        /// </summary>
        /// <response code="200">User has been successfully authenticated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("sign-in")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            var command = new SignInCommand(request.Email.ToLower(), request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return Ok(_cache.GetJwtToken(command.CacheTokenId));
        }
    }
}