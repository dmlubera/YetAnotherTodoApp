using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Extensions;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Authorize]
    [Route("api/todolists/")]
    public class TodoListsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICache _cache;

        public TodoListsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICache cache)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
        }

        /// <summary>
        /// Gets all todo lists for authenticated user
        /// </summary>
        /// <response code="200">Returned all todo lists for the authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodoListsAsync()
        {
            var todoLists = await _queryDispatcher.HandleAsync<GetTodoListsQuery, IEnumerable<TodoListDto>>(new GetTodoListsQuery(User.GetAuthenticatedUserId()));
            return Ok(todoLists);
        }

        /// <summary>
        /// Gets specified todo list for authenticated user
        /// </summary>
        /// <response code="200">Returned specified todo lists for the authenticated user</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTodoListAsync(Guid id)
        {
            var todoList = await _queryDispatcher.HandleAsync<GetTodoListQuery, DetailedTodoListDto>(new GetTodoListQuery(User.GetAuthenticatedUserId(), id));
            return Ok(todoList);
        }


        /// <summary>
        /// Created todo list
        /// </summary>
        /// <response code="201">The todo list has been created</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddTodoListAsync([FromBody] AddTodoListRequest request)
        {
            var command = new AddTodoListCommand(User.GetAuthenticatedUserId(), request.Title);
            await _commandDispatcher.DispatchAsync(command);
            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());

            return Created($"api/todolists/{resourceId}", null);
        }

        /// <summary>
        /// Updates the specified todo list
        /// </summary>
        /// <response code="200">The todo list has been updated</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{todoListId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoListAsync(Guid todoListId, [FromBody] UpdateTodoListRequest request)
        {
            var command = new UpdateTodoListCommand(User.GetAuthenticatedUserId(), todoListId, request.Title);
            await _commandDispatcher.DispatchAsync(command);
            return Ok();
        }

        /// <summary>
        /// Deletes the specified todo list
        /// </summary>
        /// <response code="204">The todo list has been deleted</response>
        /// <response code="400">An error occured while processing a request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{todoListId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTodoListAsync(Guid todoListId)
        {
            var command = new DeleteTodoListCommand(User.GetAuthenticatedUserId(), todoListId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }
    }
}