using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Helpers;

namespace YetAnotherTodoApp.Application.DI
{
    internal static class HelpersModuleInstaller
    {
        internal static void RegisterHelpersModule(this IServiceCollection services)
            => services.AddSingleton<IEncrypter, Encrypter>();
    }
}