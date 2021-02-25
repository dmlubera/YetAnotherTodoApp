using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/todolist/")]
    public class TodoListController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        
        public TodoListController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTodoListsAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            return Ok(await _queryDispatcher.HandleAsync<GetTodoListsQuery, IEnumerable<TodoListDto>>(new GetTodoListsQuery(userId)));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTodoListAsync([FromBody] CreateTodoListRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            await _commandDispatcher.DispatchAsync(new CreateTodoListCommand(userId, request.Title));
            return Ok();
        }

        [Authorize]
        [HttpPut("{todolistId}")]
        public async Task<IActionResult> UpdateTodoListAsync(Guid todoListId, [FromBody] UpdateTodoListRequest request)
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var command = new UpdateTodoListCommand
            {
                UserId = userId,
                TodoListId = todoListId,
                Title = request.Title
            };
            await _commandDispatcher.DispatchAsync(command);

            return Ok();
        }
    }
}
