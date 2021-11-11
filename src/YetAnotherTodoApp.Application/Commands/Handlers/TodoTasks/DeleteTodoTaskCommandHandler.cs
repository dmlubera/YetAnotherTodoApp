using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks
{
    public class DeleteTodoTaskCommandHandler : ICommandHandler<DeleteTodoTaskCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<DeleteTodoTaskCommandHandler> _logger;

        public DeleteTodoTaskCommandHandler(ITodoRepository repository, ILogger<DeleteTodoTaskCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

        public async Task HandleAsync(DeleteTodoTaskCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);

            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            todo.DeleteTask(command.TaskId);
            await _repository.SaveChangesAsync();
        }
    }
}