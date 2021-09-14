using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class UpdateTodoStatusCommandHandler : ICommandHandler<UpdateTodoStatusCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<UpdateTodoStatusCommandHandler> _logger;

        public UpdateTodoStatusCommandHandler(ITodoRepository repository, ILogger<UpdateTodoStatusCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateTodoStatusCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            todo.UpdateStatus(command.Status);
            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Status of todo with ID: {todo.Id} has been updated to {todo.Status}.");
        }
    }
}