using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Commands.Handlers;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Infrastructure.Auth.Helpers;

namespace YetAnotherTodoApp.Infrastructure.Auth.DI
{
    public static class AuthModuleInstaller
    {
        public static void RegisterAuthModule(this IServiceCollection services)
        {
            services.AddScoped<IJwtHelper, JwtHelper>();
        }
    }
}