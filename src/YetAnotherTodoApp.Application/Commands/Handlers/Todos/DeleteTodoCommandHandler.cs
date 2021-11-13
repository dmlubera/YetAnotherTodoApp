using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
    {
        private readonly ITodoListRepository _repository;
        private readonly ILogger<DeleteTodoCommandHandler> _logger;

        public DeleteTodoCommandHandler(ITodoListRepository repository, ILogger<DeleteTodoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteTodoCommand command)
        {
            var todoList = await _repository.GetByBelongTodo(command.UserId, command.TodoId);
            if (todoList is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);
            
            todoList.DeleteTodo(command.TodoId);

            await _repository.UpdateAsync(todoList);
            
            _logger.LogTrace($"Todo with ID: {command.TodoId} has been deleted from Todo List with ID: {todoList.Id}");
        }
    }
}