using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Extensions;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.Users;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userInfo = await _queryDispatcher
                .HandleAsync<GetUserInfoQuery, UserInfoDto>(new GetUserInfoQuery { UserId = User.GetAuthenticatedUserId() });

            return Ok(userInfo);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest request)
        {
            var command = new UpdateUserInfoCommand(User.GetAuthenticatedUserId(), request.FirstName, request.LastName);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateEmailRequest request)
        {
            var command = new UpdateEmailCommand(User.GetAuthenticatedUserId(), request.Email);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePasswordEmail([FromBody] UpdatePasswordRequest request)
        {
            var command = new UpdatePasswordCommand(User.GetAuthenticatedUserId(), request.Password);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}