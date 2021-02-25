using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class AddTodoCommandHandler : ICommandHandler<AddTodoCommand>
    {
        private readonly IUserRepository _userRepository;

        public AddTodoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(AddTodoCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = new Todo(command.Title, command.FinishDate);
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

            await _userRepository.SaveChangesAsync();
        }
    }
}