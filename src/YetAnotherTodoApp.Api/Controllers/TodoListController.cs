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

        [HttpGet]
        public async Task<IActionResult> GetTodoListsAsync()
        {
            var todoLists = await _queryDispatcher
                .HandleAsync<GetTodoListsQuery, IEnumerable<TodoListDto>>(new GetTodoListsQuery(User.GetAuthenticatedUserId()));
            return Ok(todoLists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoListAsync(Guid id)
        {
            var todoList = await _queryDispatcher
                .HandleAsync<GetTodoListQuery, TodoListDto>(new GetTodoListQuery(User.GetAuthenticatedUserId(), id));
            return Ok(todoList);
        }

        [HttpPost]
        public async Task<IActionResult> AddTodoListAsync([FromBody] AddTodoListRequest request)
        {
            var command = new AddTodoListCommand(User.GetAuthenticatedUserId(), request.Title);
            await _commandDispatcher.DispatchAsync(command);
            var resourceId = _cache.Get<Guid>(command.CacheTokenId.ToString());

            return Created($"api/todolist/{resourceId}", null);
        }

        [HttpPut("{todolistId}")]
        public async Task<IActionResult> UpdateTodoListAsync(Guid todoListId, [FromBody] UpdateTodoListRequest request)
        {
            var command = new UpdateTodoListCommand(User.GetAuthenticatedUserId(), todoListId, request.Title);
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }

        [HttpDelete("{todoListId}")]
        public async Task<IActionResult> DeleteTodoListAsync(Guid todoListId)
        {
            var command = new DeleteTodoListCommand(User.GetAuthenticatedUserId(), todoListId);
            await _commandDispatcher.DispatchAsync(command);

            return NoContent();
        }
    }
}