using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateTodoStatusCommandHandler : ICommandHandler<UpdateTodoStatusCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateTodoStatusCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateTodoStatusCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todo = user.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Id == command.TodoId);
            todo.UpdateStatus(command.Status);

            await _userRepository.SaveChangesAsync();
        }
    }
}
