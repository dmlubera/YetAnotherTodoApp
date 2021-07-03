using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class DeleteTodoListCommandHandler : ICommandHandler<DeleteTodoListCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteTodoListCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(DeleteTodoListCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todoList = user.TodoLists.FirstOrDefault(x => x.Id == command.TodoListId);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(command.TodoListId);

            user.DeleteTodoList(todoList);

            await _userRepository.SaveChangesAsync();
        }
    }
}