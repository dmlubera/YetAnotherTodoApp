using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class UpdateTodoCommandHandler : ICommandHandler<UpdateTodoCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<UpdateTodoCommandHandler> _logger;

        public UpdateTodoCommandHandler(ITodoRepository repository, ILogger<UpdateTodoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateTodoCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);
            todo.Update(command.Title, command.Description, command.FinishDate);

            await _repository.UpdateAsync(todo);

            _logger.LogTrace($"Todo with ID: {todo.Id} has been updated.");
        }
    }
}