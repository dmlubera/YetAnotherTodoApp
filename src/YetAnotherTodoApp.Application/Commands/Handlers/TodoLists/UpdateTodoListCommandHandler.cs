using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class UpdateTodoListCommandHandler : ICommandHandler<UpdateTodoListCommand>
    {
        private readonly ITodoListRepository _repository;
        private readonly ILogger<UpdateTodoListCommandHandler> _logger;

        public UpdateTodoListCommandHandler(ITodoListRepository repository, ILogger<UpdateTodoListCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateTodoListCommand command)
        {
            if (await _repository.CheckIfUserHasGotTodoListWithGivenTitle(command.UserId, command.Title))
                throw new TodoListWithGivenTitleAlreadyExistsException(command.Title);

            var todoList = await _repository.GetForUserAsync(command.UserId, command.TodoListId);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(command.TodoListId);
            if (todoList.Title == "Inbox")
                throw new InboxModificationIsNotAllowedException();

            todoList.UpdateTitle(command.Title);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Todo List with ID: {todoList.Id} has been updated.");
        }
    }
}