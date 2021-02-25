using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
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
            user.RemoveTodoList(todoList);

            await _userRepository.SaveChangesAsync();
        }
    }
}
