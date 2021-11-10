using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks
{
    public class UpdateTodoTaskCommandHandler : ICommandHandler<UpdateTodoTaskCommand>
    {
        private readonly ITodoTaskRepository _repository;
        private readonly ILogger<UpdateTodoTaskCommandHandler> _logger;

        public UpdateTodoTaskCommandHandler(ITodoTaskRepository repository, ILogger<UpdateTodoTaskCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

        public async Task HandleAsync(UpdateTodoTaskCommand command)
        {
            var task = await _repository.GetForUserAsync(command.TaskId, command.UserId);
            if (task is null)
                throw new TodoTaskWithGivenIdDoesNotExistException(command.TaskId);

            task.Update(command.Title, command.Description);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Task with ID: {task.Id} has been updated.");
        }
    }
}