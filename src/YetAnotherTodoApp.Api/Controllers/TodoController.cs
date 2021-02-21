using System;
using System.Collections.Generic;
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
    [Route("api/todo/")]
    public class TodoController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public TodoController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTodosAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var todos = await _queryDispatcher.HandleAsync<GetTodosQuery, IEnumerable<TodoDto>>(new GetTodosQuery(userId));
            return Ok(todos);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTodoAsync([FromBody] AddTodoRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new AddTodoCommand
            {
                UserId = userId,
                Title =  request.Title,
                Project = request.Project,
                FinishDate = request.FinishDate
            };
            await _commandDispatcher.DispatchAsync(command);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{todoId}")]
        public async Task<IActionResult> DeleteTodoAsync(Guid todoId)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new DeleteTodoCommand
            {
                UserId = userId,
                TodoId = todoId
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("{todoId}/status")]
        public async Task<IActionResult> UpdateTodoStatusAsync(Guid todoId, [FromBody] UpdateTodoStatusRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateTodoStatusCommand
            {
                UserId = userId,
                TodoId = todoId,
                Status = request.Status
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpPut("{todoId}/priority")]
        public async Task<IActionResult> UpdateTodoPriorityAsync(Guid todoId, [FromBody] UpdateTodoPriorityRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateTodoPriorityCommand
            {
                UserId = userId,
                TodoId = todoId,
                Priority = request.Priority
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}