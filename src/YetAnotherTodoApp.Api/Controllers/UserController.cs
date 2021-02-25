using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Models;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/users/")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IMemoryCache _cache;

        public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IMemoryCache cache)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var userInfo = await _queryDispatcher.HandleAsync<GetUserInfoQuery, UserInfoDto>(new GetUserInfoQuery { UserId = userId });

            return Ok(userInfo);
        }

        [HttpPost]
        public async Task RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateUserInfoCommand
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateUserEmailAsync([FromBody] UpdateUserEmailRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateUserEmailCommand
            {
                UserId = userId,
                Email = request.Email
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPasswordEmail([FromBody] UpdateUserPasswordRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateUserPasswordCommand
            {
                UserId = userId,
                Password = request.Password
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}