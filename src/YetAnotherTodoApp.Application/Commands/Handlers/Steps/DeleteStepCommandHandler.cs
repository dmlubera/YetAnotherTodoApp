using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class DeleteStepCommandHandler : ICommandHandler<DeleteStepCommand>
    {
        private readonly ITodoRepository _repository;

        public DeleteStepCommandHandler(ITodoRepository repository)
            => _repository = repository;

        public async Task HandleAsync(DeleteStepCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);

            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);
            
            todo.RemoveStep(command.StepId);
            await _repository.SaveChangesAsync();
        }
    }
}