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
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public UsersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        /// <summary>
        /// Gets user info of authenticated user
        /// </summary>
        /// <response code="200">Returned user info of authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var userInfo = await _queryDispatcher.HandleAsync<GetUserInfoQuery, UserInfoDto>(new GetUserInfoQuery { UserId = User.GetAuthenticatedUserId() });
            return Ok(userInfo);
        }

        /// <summary>
        /// Updates user info
        /// </summary>
        /// <response code="200">The user info has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserInfoAsync([FromBody] UpdateUserInfoRequest request)
        {
            var command = new UpdateUserInfoCommand(User.GetAuthenticatedUserId(), request.FirstName, request.LastName);
            await _commandDispatcher.DispatchAsync(command);
            return Ok();
        }

        /// <summary>
        /// Changes email address
        /// </summary>
        /// <response code="200">The email address has been changed</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("email")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateEmailRequest request)
        {
            var command = new UpdateEmailCommand(User.GetAuthenticatedUserId(), request.Email);
            await _commandDispatcher.DispatchAsync(command);
            return Ok();
        }

        /// <summary>
        /// Changes password
        /// </summary>
        /// <response code="200">The password address has been changed</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePasswordEmail([FromBody] UpdatePasswordRequest request)
        {
            var command = new UpdatePasswordCommand(User.GetAuthenticatedUserId(), request.Password);
            await _commandDispatcher.DispatchAsync(command);
            return Ok();
        }
    }
}