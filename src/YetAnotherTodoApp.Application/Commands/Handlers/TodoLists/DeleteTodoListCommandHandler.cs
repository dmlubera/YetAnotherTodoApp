using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class DeleteTodoListCommandHandler : ICommandHandler<DeleteTodoListCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<DeleteTodoListCommandHandler> _logger;

        public DeleteTodoListCommandHandler(IUserRepository repository, ILogger<DeleteTodoListCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteTodoListCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);

            if (user is null)
                throw new UserNotExistException(command.UserId);

            user.DeleteTodoList(command.TodoListId);

            await _repository.UpdateAsync(user);

            _logger.LogTrace($"Todo List with ID: {command.TodoListId} has been deleted.");
        }
    }
}