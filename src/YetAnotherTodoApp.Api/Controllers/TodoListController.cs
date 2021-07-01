using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/todolist/")]
    public class TodoListController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICache _cache;

        public TodoListController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICache cache)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTodoListsAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            return Ok(await _queryDispatcher.HandleAsync<GetTodoListsQuery, IEnumerable<TodoListDto>>(new GetTodoListsQuery(userId)));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoListAsync(Guid id)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            return Ok(await _queryDispatcher.HandleAsync<GetTodoListQuery, TodoListDto>(new GetTodoListQuery(userId, id)));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] CreateTodoListRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new CreateTodoListCommand(userId, request.Title);
            await _commandDispatcher.DispatchAsync(command);
            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());

            return Created($"api/todolist/{resourceId}", null);
        }

        [Authorize]
        [HttpPut("{todolistId}")]
        public async Task<IActionResult> UpdateTodoListAsync(Guid todoListId, [FromBody] UpdateTodoListRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateTodoListCommand(userId, todoListId, request.Title);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{todoListId}")]
        public async Task<IActionResult> DeleteTodoListAsync(Guid todoListId)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new DeleteTodoListCommand(userId, todoListId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }
    }
}