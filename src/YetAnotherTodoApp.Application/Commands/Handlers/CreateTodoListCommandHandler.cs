using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class CreateTodoListCommandHandler : ICommandHandler<CreateTodoListCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateTodoListCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task HandleAsync(CreateTodoListCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);

            user.AddTodoList(new TodoList(command.Title));
            await _userRepository.SaveChangesAsync();
        }
    }
}
