using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.Users;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/users/")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UserController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var userInfo = await _queryDispatcher.HandleAsync<GetUserInfoQuery, UserInfoDto>(new GetUserInfoQuery { UserId = userId });

            return Ok(userInfo);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateUserInfoCommand(userId, request.FirstName, request.LastName);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateEmailRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateEmailCommand(userId, request.Email);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("password")]
        public async Task<IActionResult> UpdatePasswordEmail([FromBody] UpdatePasswordRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdatePasswordCommand(userId, request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}