using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Users
{
    public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IUserRepository _repository;
        public UpdateUserInfoCommandHandler(IUserRepository repository) 
            => _repository = repository;

        public async Task HandleAsync(UpdateUserInfoCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            user.UpdateUserInfo(command.FirstName, command.LastName);

            await _repository.SaveChangesAsync();
        }
    }
}