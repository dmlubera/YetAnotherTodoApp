using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Handlers.Auths;
using YetAnotherTodoApp.Application.Commands.Handlers.Steps;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Handlers.Users;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Users;

namespace YetAnotherTodoApp.Application.DI
{
    internal static class CommandsModuleInstaller
    {
        internal static void RegisterCommandsModule(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<SignUpCommand>, SignUpCommandHandler>();
            services.AddTransient<ICommandHandler<AddTodoCommand>, AddTodoCommandHandler>();
            services.AddTransient<ICommandHandler<AddTodoListCommand>, AddTodoListCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteTodoCommand>, DeleteTodoCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateTodoStatusCommand>, UpdateTodoStatusCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateTodoPriorityCommand>, UpdateTodoPriorityCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateTodoCommand>, UpdateTodoCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateUserInfoCommand>, UpdateUserInfoCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateEmailCommand>, UpdateEmailCommandHandler>();
            services.AddTransient<ICommandHandler<UpdatePasswordCommand>, UpdatePasswordCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateTodoListCommand>, UpdateTodoListCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteTodoListCommand>, DeleteTodoListCommandHandler>();
            services.AddTransient<ICommandHandler<SignInCommand>, SignInCommandHandler>();
            services.AddTransient<ICommandHandler<AddStepCommand>, AddStepCommandHandler>();
            services.AddTransient<ICommandHandler<DeleteStepCommand>, DeleteStepCommandHandler>();
            services.AddTransient<ICommandHandler<CompleteStepCommand>, CompleteStepCommandHandler>();
            services.AddTransient<ICommandHandler<UpdateStepCommand>, UpdateStepCommandHandler>();
        }
    }
}