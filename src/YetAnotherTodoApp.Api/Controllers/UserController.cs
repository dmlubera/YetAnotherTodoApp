using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models;

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