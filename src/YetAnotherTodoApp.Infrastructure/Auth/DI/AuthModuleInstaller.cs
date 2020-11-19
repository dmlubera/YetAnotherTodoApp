using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Handlers;
using YetAnotherTodoApp.Infrastructure.Auth.Commands.Models;
using YetAnotherTodoApp.Infrastructure.Auth.Helpers;

namespace YetAnotherTodoApp.Infrastructure.Auth.DI
{
    public static class AuthModuleInstaller
    {
        public static void RegisterAuthModule(this IServiceCollection services)
        {
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<ICommandHandler<LoginUserCommand>, LoginUserCommandHandler>();
        }
    }
}