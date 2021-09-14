using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class CompleteStepCommandHandler : ICommandHandler<CompleteStepCommand>
    {
        private readonly IStepRepository _repository;
        private readonly ILogger<CompleteStepCommandHandler> _logger;

        public CompleteStepCommandHandler(IStepRepository repository, ILogger<CompleteStepCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

        public async Task HandleAsync(CompleteStepCommand command)
        {
            var step = await _repository.GetForUserAsync(command.StepId, command.UserId);
            if (step is null)
                throw new StepWithGivenIdDoesNotExistException(command.StepId);

            step.Complete();

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Step with ID: {step.Id} has been completed.");
        }
    }
}