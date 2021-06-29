using System;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class AddTodoCommandHandler : ICommandHandler<AddTodoCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICache _cache;

        public AddTodoCommandHandler(IUserRepository userRepository, ICache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task HandleAsync(AddTodoCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = new Todo(command.Title, command.FinishDate);
            if (command.Steps.Count != 0)
                todo.AddSteps(command.Steps.Select(x => new Step(x.Title, x.Description)).ToList());

            if (string.IsNullOrWhiteSpace(command.Project))
                user.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox")
                    ?.AddTodo(todo);
            else
            {
                var todoList = user.TodoLists.FirstOrDefault(x => x.Title.Value == command.Project);
                if(todoList != null)
                    todoList.AddTodo(todo);
                else
                {
                    var newTodoList = new TodoList(command.Project);
                    newTodoList.AddTodo(todo);
                    user.AddTodoList(newTodoList);
                }
            }

            _cache.Set(command.CacheId.ToString(), todo.Id, TimeSpan.FromSeconds(99));

            await _userRepository.SaveChangesAsync();
        }
    }
}