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
    }
}