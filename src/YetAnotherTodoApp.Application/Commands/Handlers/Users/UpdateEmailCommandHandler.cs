using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers.Users
{
    public class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand>
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;

        public UpdateEmailCommandHandler(IUserRepository repository, ILogger<UpdateEmailCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateEmailCommand command)
        {
            if (await _repository.CheckIfEmailIsInUseAsync(command.Email))
                throw new EmailInUseException(command.Email);
            var user = await _repository.GetByIdAsync(command.UserId);

            user.UpdateEmail(command.Email);

            await _repository.SaveChangesAsync();

            _logger.LogTrace($"Email of user with ID: {user.Id} has been updated.");
        }
    }
}