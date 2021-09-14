using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class DeleteStepCommandHandler : ICommandHandler<DeleteStepCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<DeleteStepCommandHandler> _logger;

        public DeleteStepCommandHandler(ITodoRepository repository, ILogger<DeleteStepCommandHandler> logger)
            => (_repository, _logger) = (repository, logger);

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