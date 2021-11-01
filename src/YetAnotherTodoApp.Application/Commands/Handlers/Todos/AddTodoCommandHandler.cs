using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Todos
{
    public class AddTodoCommandHandler : ICommandHandler<AddTodoCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ICache _cache;
        private readonly ILogger<AddTodoCommandHandler> _logger;

        public AddTodoCommandHandler(IUserRepository repository, ICache cache, ILogger<AddTodoCommandHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task HandleAsync(AddTodoCommand command)
        {
            TodoList todoList;
            var user = await _repository.GetByIdAsync(command.UserId);
            var todo = new Todo(command.Title, command.FinishDate, command.Description,
                command.Priority ?? TodoPriority.Normal);
            
            if (command.Steps.Count != 0)
                todo.AddSteps(command.Steps.Select(x => new Step(x.Title, x.Description)).ToList());

            if (string.IsNullOrWhiteSpace(command.Project))
            {
                todoList = user.TodoLists.FirstOrDefault(x => x.Title == "Inbox");
                todoList.AddTodo(todo);
            }
            else
            {
                todoList = user.TodoLists.FirstOrDefault(x => x.Title == command.Project);
                if (todoList != null)
                    todoList.AddTodo(todo);
                else
                {
                    todoList = new TodoList(command.Project);
                    todoList.AddTodo(todo);
                    user.AddTodoList(todoList);
                }
            }

            _cache.Set(command.CacheTokenId.ToString(), todo.Id, TimeSpan.FromSeconds(99));

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Todo with ID: {todo.Id} has been added to Todo List with ID: {todoList.Id}");
        }
    }
}