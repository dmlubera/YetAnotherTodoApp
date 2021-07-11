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
            
            var todoList = user.TodoLists.FirstOrDefault(x => x.Todos.Any(x => x.Id == command.TodoId));
            if (todoList is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            todoList.DeleteTodo(command.TodoId);

            await _repository.SaveChangesAsync();
        }
    }
}