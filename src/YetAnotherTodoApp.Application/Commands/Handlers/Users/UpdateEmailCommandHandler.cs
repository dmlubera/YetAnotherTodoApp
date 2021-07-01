using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Users
{
    public class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateEmailCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UpdateEmailCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (command.Email == user.Email.Value)
                throw new UpdateEmailToAlreadyUsedValueException();

            user.UpdateEmail(command.Email);

            await _userRepository.SaveChangesAsync();
        }
    }
}