using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Users
{
    public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UpdateUserInfoCommandHandler> _logger;
        public UpdateUserInfoCommandHandler(IUserRepository repository, ILogger<UpdateUserInfoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateUserInfoCommand command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);
            user.UpdateUserInfo(command.FirstName, command.LastName);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"User's info of user with ID: {user.Id} has been updated.");
        }
    }
}