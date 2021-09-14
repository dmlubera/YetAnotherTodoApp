using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class UpdateTodoPriorityCommandHandler : ICommandHandler<UpdateTodoPriorityCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<UpdateTodoPriorityCommandHandler> _logger;

        public UpdateTodoPriorityCommandHandler(ITodoRepository repository, ILogger<UpdateTodoPriorityCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateTodoPriorityCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);
            
            if (todo.Priority != command.Priority)
            {
                todo.UpdatePriority(command.Priority);
                await _repository.SaveChangesAsync();

                _logger.LogTrace($"Priority of todo with ID: {todo.Id} has been updated to {todo.Priority}.");
            }
        }
    }
}