using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Handlers.Auths;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Handlers.Users;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Users;

namespace YetAnotherTodoApp.Application.DI
{
    public static class CommandsModuleInstaller
    {
        public static void RegisterCommandsModule(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandler<SignUpCommand>, SignUpCommandHandler>();
            services.AddScoped<ICommandHandler<AddTodoCommand>, AddTodoCommandHandler>();
            services.AddScoped<ICommandHandler<CreateTodoListCommand>, CreateTodoListCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteTodoCommand>, DeleteTodoCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoStatusCommand>, UpdateTodoStatusCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoPriorityCommand>, UpdateTodoPriorityCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoCommand>, UpdateTodoCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateUserInfoCommand>, UpdateUserInfoCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateEmailCommand>, UpdateEmailCommandHandler>();
            services.AddScoped<ICommandHandler<UpdatePasswordCommand>, UpdatePasswordCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateTodoListCommand>, UpdateTodoListCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteTodoListCommand>, DeleteTodoListCommandHandler>();
            services.AddScoped<ICommandHandler<SignInCommand>, SignInCommandHandler>();
        }
    }
}