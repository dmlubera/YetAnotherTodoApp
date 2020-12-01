using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Domain.Entities;

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

        [HttpGet]
        public async Task<IActionResult> GetTodosAsync()
        {
            var userId = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.Name) : Guid.Empty;
            var todos = await _queryDispatcher.HandleAsync<GetTodosQuery, IEnumerable<Todo>>(new GetTodosQuery { UserId = userId });
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
    }
}