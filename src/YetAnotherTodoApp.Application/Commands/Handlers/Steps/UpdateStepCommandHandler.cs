using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class UpdateStepCommandHandler : ICommandHandler<UpdateStepCommand>
    {
        private readonly IStepRepository _repository;
        private readonly ILogger<UpdateStepCommandHandler> _logger;

        public UpdateStepCommandHandler(IStepRepository repository, ILogger<UpdateStepCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

        public async Task HandleAsync(UpdateStepCommand command)
        {
            var step = await _repository.GetForUserAsync(command.StepId, command.UserId);
            if (step is null)
                throw new StepWithGivenIdDoesNotExistException(command.StepId);

            step.Update(command.Title, command.Description);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Step with ID: {step.Id} has been updated.");
        }
    }
}