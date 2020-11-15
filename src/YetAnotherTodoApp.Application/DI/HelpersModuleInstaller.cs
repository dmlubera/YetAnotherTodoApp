using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Helpers;

namespace YetAnotherTodoApp.Application.DI
{
    public static class HelpersModuleInstaller
    {
        public static void RegisterHelpersModule(this IServiceCollection services)
        {
            services.AddScoped<IEncrypter, Encrypter>();
        }
    }
}