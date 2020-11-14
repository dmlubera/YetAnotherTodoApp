using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Handlers;
using YetAnotherTodoApp.Application.Commands.Models;

namespace YetAnotherTodoApp.Application.DI
{
    public static class CommandsModuleInstaller
    {
        public static void RegisterCommandsModule(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<ICommandHandler<RegisterUserCommand>, RegisterUserCommandHandler>();

        }
    }
}