using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateTodoCommandHandler : ICommandHandler<UpdateTodoCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateTodoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateTodoCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = user.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Id == command.TodoId);
            todo.Update(command.Title, command.Description, command.FinishDate);
            
            await _userRepository.SaveChangesAsync();
        }
    }
}
