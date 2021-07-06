using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class DeleteTodoListCommandHandler : ICommandHandler<DeleteTodoListCommand>
    {
        private readonly IUserRepository _repository;

        public DeleteTodoListCommandHandler(IUserRepository repository) 
            => _repository = repository;

        public async Task HandleAsync(DeleteTodoListCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            var todoList = user.TodoLists.FirstOrDefault(x => x.Id == command.TodoListId);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(command.TodoListId);
            if (todoList.Title.Value == "Inbox")
                throw new InboxDeletionIsNotAllowedException();

            user.DeleteTodoList(todoList);

            await _repository.SaveChangesAsync();
        }
    }
}