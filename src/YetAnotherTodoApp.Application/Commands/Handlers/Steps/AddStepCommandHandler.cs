using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Steps
{
    public class AddStepCommandHandler : ICommandHandler<AddStepCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ICache _cache;

        public AddStepCommandHandler(ITodoRepository repository, ICache cache)
            => (_repository, _cache) = (repository, cache);

        public async Task HandleAsync(AddStepCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            var step = new Step(command.Title, command.Description);
            todo.AddSteps(new[] { step });

            _cache.Set(command.CacheTokenId.ToString(), todo.Id, TimeSpan.FromSeconds(99));

            await _repository.SaveChangesAsync();
        }
    }
}