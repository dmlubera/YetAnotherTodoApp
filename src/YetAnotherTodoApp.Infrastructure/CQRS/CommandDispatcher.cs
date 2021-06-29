using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands;

namespace YetAnotherTodoApp.Infrastructure.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            var handler = _serviceProvider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;
            await handler.HandleAsync(command);
        }
    }
}