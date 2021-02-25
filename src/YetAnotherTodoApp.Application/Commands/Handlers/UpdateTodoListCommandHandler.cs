using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateTodoListCommandHandler : ICommandHandler<UpdateTodoListCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateTodoListCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateTodoListCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todoList = user.TodoLists.FirstOrDefault(x => x.Id == command.TodoListId);
            todoList.UpdateTitle(command.Title);

            await _userRepository.SaveChangesAsync();
        }
    }
}
