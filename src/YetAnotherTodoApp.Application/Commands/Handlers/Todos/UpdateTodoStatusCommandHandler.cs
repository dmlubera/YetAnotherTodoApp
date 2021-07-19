using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class UpdateTodoStatusCommandHandler : ICommandHandler<UpdateTodoStatusCommand>
    {
        private readonly ITodoRepository _repository;

        public UpdateTodoStatusCommandHandler(ITodoRepository repository)
            => _repository = repository;

        public async Task HandleAsync(UpdateTodoStatusCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            todo.UpdateStatus(command.Status);
            await _repository.SaveChangesAsync();
        }
    }
}