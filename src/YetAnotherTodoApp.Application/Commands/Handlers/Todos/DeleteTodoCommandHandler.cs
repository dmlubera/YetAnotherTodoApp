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

        public DeleteTodoCommandHandler(IUserRepository repository) 
            => _repository = repository;

        public async Task HandleAsync(DeleteTodoCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            var todo = user.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Id == command.TodoId);

            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            user.TodoLists.FirstOrDefault(x => x.Title.Value == todo.TodoList.Title.Value).DeleteTodo(todo);

            await _repository.SaveChangesAsync();
        }
    }
}