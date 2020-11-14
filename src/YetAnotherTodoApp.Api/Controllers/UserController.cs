using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Models;

namespace YetAnotherTodoApp.Api.Controllers
{
    [Route("api/users/")]
    public class UserController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UserController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task RegisterUserAsync([FromBody] RegisterUserCommand command)
            => await _commandDispatcher.DispatchAsync(command);

    }
}