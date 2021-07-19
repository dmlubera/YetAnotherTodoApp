using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Infrastructure.Auth.Helpers;

namespace YetAnotherTodoApp.Infrastructure.Auth.DI
{
    internal static class AuthModuleInstaller
    {
        internal static void RegisterAuthModule(this IServiceCollection services)
            => services.AddSingleton<IJwtHelper, JwtHelper>();
    }
}