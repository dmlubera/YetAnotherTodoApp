using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Cache;
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
        private readonly ICache _cache;

        public TodoController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICache cache)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoAsync(Guid id)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var todo = await _queryDispatcher.HandleAsync<GetTodoQuery, TodoDto>(new GetTodoQuery(userId, id));

            return Ok(todo);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTodoAsync([FromBody] AddTodoRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new AddTodoCommand(userId, request.Title, request.Project, request.FinishDate);
            await _commandDispatcher.DispatchAsync(command);

            var resourceId = _cache.Get<Guid>(command.CacheId.ToString());
            return Created($"/api/todo/{resourceId}", null);
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

            return NoContent();
        }

        [Authorize]
        [HttpPut("{todoId}")]
        public async Task<IActionResult> UpdateTodoAsync(Guid todoId, [FromBody] UpdateTodoRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateTodoCommand
            {
                UserId = userId,
                TodoId = todoId,
                Title = request.Title,
                Description = request.Description,
                FinishDate = request.FinishDate
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