using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class UpdateTodoPriorityCommandHandler : ICommandHandler<UpdateTodoPriorityCommand>
    {
        private readonly ITodoRepository _repository;

        public UpdateTodoPriorityCommandHandler(ITodoRepository repository)
            => _repository = repository;

        public async Task HandleAsync(UpdateTodoPriorityCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);
            
            if (todo.Priority != command.Priority)
            {
                todo.UpdatePriority(command.Priority);
                await _repository.SaveChangesAsync();
            }
        }
    }
}