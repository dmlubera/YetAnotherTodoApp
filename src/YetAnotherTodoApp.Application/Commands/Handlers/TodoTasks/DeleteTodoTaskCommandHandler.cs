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
            var todo = await _repository.GetForUserByTodoTaskId(command.TaskId, command.UserId);

            if (todo is null)
                throw new TodoTaskWithGivenIdDoesNotExistException(command.TaskId);

            todo.DeleteTask(command.TaskId);

            await _repository.UpdateAsync(todo);
            _logger.LogTrace($"Task with ID: {command.TaskId} has been deleted from Todo List with ID: {todo.Id}");
        }
    }
}