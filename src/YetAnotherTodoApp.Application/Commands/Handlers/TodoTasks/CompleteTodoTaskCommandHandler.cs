using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks
{
    public class CompleteTodoTaskCommandHandler : ICommandHandler<CompleteTodoTaskCommand>
    {
        private readonly ITodoTaskRepository _repository;
        private readonly ILogger<CompleteTodoTaskCommandHandler> _logger;

        public CompleteTodoTaskCommandHandler(ITodoTaskRepository repository, ILogger<CompleteTodoTaskCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

        public async Task HandleAsync(CompleteTodoTaskCommand command)
        {
            var task = await _repository.GetForUserAsync(command.TaskId, command.UserId);
            if (task is null)
                throw new TodoTaskWithGivenIdDoesNotExistException(command.TaskId);

            task.Complete();

            await _repository.UpdateAsync(task);

            _logger.LogTrace($"Task with ID: {task.Id} has been completed.");
        }
    }
}