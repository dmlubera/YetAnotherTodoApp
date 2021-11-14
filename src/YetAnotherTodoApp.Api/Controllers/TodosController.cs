using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Extensions;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Api.Models.TodoTasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.Todos;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Authorize]
    [Route("api/todos/")]
    public class TodosController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICache _cache;
        private readonly IMapper _mapper;

        public TodosController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,
            ICache cache, IMapper mapper)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all todos for authenticated user
        /// </summary>
        /// <response code="200">Returned all todos for the authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodosAsync()
        {
            var todos = await _queryDispatcher
                .HandleAsync<GetTodosQuery, IEnumerable<TodoDto>>(new GetTodosQuery(User.GetAuthenticatedUserId()));
            return Ok(todos);
        }

        /// <summary>
        /// Gets specified todo for authenticated user
        /// </summary>
        /// <response code="200">Returned specified todo for the authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodoAsync(Guid id)
        {
            var todo = await _queryDispatcher
                .HandleAsync<GetTodoQuery, DetailedTodoDto>(new GetTodoQuery(User.GetAuthenticatedUserId(), id));

            return Ok(todo);
        }


        /// <summary>
        /// Creates todo for authenticated user
        /// </summary>
        /// <response code="201">The todo has been created</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddTodoAsync([FromBody] AddTodoRequest request)
        {
            var command = new AddTodoCommand(User.GetAuthenticatedUserId(), request.Title,
                request.TodoList, request.FinishDate, request.Description, request.Priority,
                _mapper.Map<IEnumerable<TodoTaskDto>>(request.Tasks));

            await _commandDispatcher.DispatchAsync(command);

            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());
            return Created($"/api/todos/{resourceId}", null);
        }

        /// <summary>
        /// Deletes the specified todo
        /// </summary>
        /// <response code="204">The todo has been deleted</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{todoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTodoAsync(Guid todoId)
        {
            var command = new DeleteTodoCommand(User.GetAuthenticatedUserId(), todoId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        /// <summary>
        /// Updates the specified todo
        /// </summary>
        /// <response code="200">The todo has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{todoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoAsync(Guid todoId, [FromBody] UpdateTodoRequest request)
        {
            var command = new UpdateTodoCommand(User.GetAuthenticatedUserId(), todoId,
                request.Title, request.Description, request.FinishDate);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        /// <summary>
        /// Updates the status of the specified todo
        /// </summary>
        /// <response code="200">The todo's status has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{todoId}/status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoStatusAsync(Guid todoId, [FromBody] UpdateTodoStatusRequest request)
        {
            var command = new UpdateTodoStatusCommand(User.GetAuthenticatedUserId(), todoId, request.Status);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        /// <summary>
        /// Updates the priority of the specified todo
        /// </summary>
        /// <response code="200">The todo's priority has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{todoId}/priority")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoPriorityAsync(Guid todoId, [FromBody] UpdateTodoPriorityRequest request)
        {
            var command = new UpdateTodoPriorityCommand(User.GetAuthenticatedUserId(), todoId, request.Priority);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        /// <summary>
        /// Adds the task to the specified todo
        /// </summary>
        /// <response code="201">The task has been added to the specified todo</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("{todoId}/tasks")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddTodoTaskAsync(Guid todoId, [FromBody] AddTodoTaskRequest request)
        {
            var command = new AddTodoTaskCommand(User.GetAuthenticatedUserId(), todoId, request.Title, request.Description);
            await _commandDispatcher.DispatchAsync(command);

            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());
            return Created($"/api/todos/{resourceId}", null);
        }


        /// <summary>
        /// Deletes the taks from the specified todo
        /// </summary>
        /// <response code="204">The task has been deleted from the specified todo</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{todoId}/tasks/{taskId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTodoTaskAsync(Guid todoId, Guid taskId)
        {
            var command = new DeleteTodoTaskCommand(User.GetAuthenticatedUserId(), todoId, taskId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }

        /// <summary>
        /// Completes the specified task
        /// </summary>
        /// <response code="200">The task has been completed</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("tasks/{taskId}/complete")]
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
        [HttpPut("tasks/{taskId}")]
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