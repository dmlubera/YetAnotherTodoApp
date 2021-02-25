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
            services.AddScoped<ICommandHandler<AddTodoCommand>, AddTodoCommandHandler>();
            services.AddScoped<ICommandHandler<CreateTodoListCommand>, CreateTodoListCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteTodoCommand>, DeleteTodoCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoStatusCommand>, UpdateTodoStatusCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoPriorityCommand>, UpdateTodoPriorityCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoCommand>, UpdateTodoCommandHandler>();
        }
    }
}