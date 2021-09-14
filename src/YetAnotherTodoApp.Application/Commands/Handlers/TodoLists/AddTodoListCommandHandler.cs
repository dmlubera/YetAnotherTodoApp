using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class AddTodoListCommandHandler : ICommandHandler<AddTodoListCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ICache _cache;
        private readonly ILogger<AddTodoListCommandHandler> _logger;

        public AddTodoListCommandHandler(IUserRepository repository, ICache cache,
            ILogger<AddTodoListCommandHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(AddTodoListCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            var todoList = new TodoList(command.Title);
            user.AddTodoList(todoList);
            await _repository.SaveChangesAsync();

            _cache.Set(command.CacheTokenId.ToString(), todoList.Id, TimeSpan.FromSeconds(99));

            _logger.LogTrace($"Todo List with ID: {todoList.Id} has been added to user with ID: {user.Id}");
        }
    }
}