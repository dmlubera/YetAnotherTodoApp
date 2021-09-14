using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<DeleteTodoCommandHandler> _logger;

        public DeleteTodoCommandHandler(IUserRepository repository, ILogger<DeleteTodoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteTodoCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            
            var todoList = user.TodoLists.FirstOrDefault(x => x.Todos.Any(x => x.Id == command.TodoId));
            if (todoList is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            todoList.DeleteTodo(command.TodoId);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Todo with ID: {command.TodoId} has been deleted from Todo List with ID: {todoList.Id}");
        }
    }
}