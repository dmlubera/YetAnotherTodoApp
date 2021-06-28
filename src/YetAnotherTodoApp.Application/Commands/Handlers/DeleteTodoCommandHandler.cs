using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
    {
        private readonly IUserRepository _userRepository;

        public DeleteTodoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(DeleteTodoCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = user.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Id == command.TodoId);

            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            user.TodoLists.FirstOrDefault(x => x.Title.Value == todo.TodoList.Title.Value).DeleteTodo(todo);

            await _userRepository.SaveChangesAsync();
        }
    }
}
