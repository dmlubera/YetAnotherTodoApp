using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateUserInfoCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            user.UpdateUserInfo(command.FirstName, command.LastName);

            await _userRepository.SaveChangesAsync();
        }
    }
}
