using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks
{
    public class AddTodoTaskCommandHandler : ICommandHandler<AddTodoTaskCommand>
    {
        private readonly ITodoRepository _repository;
        private readonly ICache _cache;
        private readonly ILogger<AddTodoTaskCommandHandler> _logger;

        public AddTodoTaskCommandHandler(ITodoRepository repository, ICache cache,
            ILogger<AddTodoTaskCommandHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(AddTodoTaskCommand command)
        {
            var todo = await _repository.GetForUserAsync(command.TodoId, command.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(command.TodoId);

            var task = new TodoTask(command.Title, command.Description);
            todo.AddTasks(new[] { task });

            _cache.Set(command.CacheTokenId.ToString(), todo.Id, TimeSpan.FromSeconds(99));

            await _repository.UpdateAsync(todo);

            _logger.LogTrace($"Task with ID: {task.Id} has been added to Todo with ID: {todo.Id}");
        }
    }
}