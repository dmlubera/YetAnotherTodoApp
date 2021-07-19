using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class CompleteStepCommandHandler : ICommandHandler<CompleteStepCommand>
    {
        private readonly IStepRepository _repository;

        public CompleteStepCommandHandler(IStepRepository repository)
            => _repository = repository;

        public async Task HandleAsync(CompleteStepCommand command)
        {
            var step = await _repository.GetForUserAsync(command.StepId, command.UserId);
            if (step is null)
                throw new StepWithGivenIdDoesNotExistException(command.StepId);

            step.Complete();

            await _repository.SaveChangesAsync();
        }
    }
}