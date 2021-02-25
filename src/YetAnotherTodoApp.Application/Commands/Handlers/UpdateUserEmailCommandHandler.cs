using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateUserEmailCommandHandler : ICommandHandler<UpdateUserEmailCommand>
    {
        private readonly IUserRepository _userRepository;
        
        public UpdateUserEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateUserEmailCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            user.UpdateEmail(command.Email);

            await _userRepository.SaveChangesAsync();
        }
    }
}
