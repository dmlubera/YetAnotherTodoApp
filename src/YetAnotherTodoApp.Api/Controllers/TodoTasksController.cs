using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Extensions;
using YetAnotherTodoApp.Api.Models.TodoTasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.TodoTasks;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/todoTasks/")]
    public class TodoTasksController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ICache _cache;

        public TodoTasksController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, ICache cache)
        {
            _queryDispatcher = queryDispatcher;
            _commandDispatcher = commandDispatcher;
            _cache = cache;
        }

        /// <summary>
        /// Adds the task to the specified todo
        /// </summary>
        /// <response code="201">The task has been added to the specified todo</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddTodoTaskAsync([FromBody] AddTodoTaskRequest request)
        {
            var command = new AddTodoTaskCommand(User.GetAuthenticatedUserId(), request.TodoId, request.Title, request.Description);
            await _commandDispatcher.DispatchAsync(command);

            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());
            return Created($"/api/todos/{resourceId}", null);
        }

        /// <summary>
        /// Gets specified task for authenticated user
        /// </summary>
        /// <response code="200">Returned specified task for the authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodoTaskAsync(Guid id)
        {
            var todo = await _queryDispatcher
                .HandleAsync<GetTodoTaskQuery, TodoTaskDto>(new GetTodoTaskQuery(User.GetAuthenticatedUserId(), id));

            return Ok(todo);
        }


        /// <summary>
        /// Deletes the task from the specified todo
        /// </summary>
        /// <response code="204">The task has been deleted from the specified todo</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{taskId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTodoTaskAsync(Guid taskId)
        {
            var command = new DeleteTodoTaskCommand(User.GetAuthenticatedUserId(), taskId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        /// <summary>
        /// Completes the specified task
        /// </summary>
        /// <response code="200">The task has been completed</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{taskId:guid}/complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CompleteTodoTaskAsync(Guid taskId)
        {
            var command = new CompleteTodoTaskCommand(User.GetAuthenticatedUserId(), taskId);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        /// <summary>
        /// Updates the specified task
        /// </summary>
        /// <response code="200">The task has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{taskId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoTaskAsync(Guid taskId, [FromBody] UpdateTodoTaskRequest request)
        {
            var command = new UpdateTodoTaskCommand(User.GetAuthenticatedUserId(), taskId, request.Title, request.Description);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}