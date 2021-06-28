using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateTodoPriorityCommandHandler : ICommandHandler<UpdateTodoPriorityCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateTodoPriorityCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateTodoPriorityCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = user.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Id == command.TodoId);
            
            if(todo.Priority != command.Priority)
            {
                todo.UpdatePriority(command.Priority);
                await _userRepository.SaveChangesAsync();
            }
        }
    }
}
