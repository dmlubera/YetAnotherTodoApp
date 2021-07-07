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

            user.DeleteTodoList(command.TodoListId);

            await _repository.SaveChangesAsync();
        }
    }
}