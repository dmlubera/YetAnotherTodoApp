using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.TodoLists
{
    public class AddTodoListCommandHandler : ICommandHandler<AddTodoListAsync>
    {
        private readonly IUserRepository _repository;
        private readonly ICache _cache;

        public AddTodoListCommandHandler(IUserRepository repository, ICache cache)
            => (_repository, _cache) = (repository, cache);

        public async Task HandleAsync(AddTodoListAsync command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            var todoList = new TodoList(command.Title);
            user.AddTodoList(todoList);
            await _repository.SaveChangesAsync();

            _cache.Set(command.CacheTokenId.ToString(), todoList.Id, TimeSpan.FromSeconds(99));
        }
    }
}