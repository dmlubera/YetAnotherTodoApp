using Microsoft.Extensions.DependencyInjection;

namespace YetAnotherTodoApp.Application.DI
{
    public static class ApplicationModuleInstaller
    {
        public static void RegisterApplicationModule(this IServiceCollection services)
        {
            services.RegisterAutoMapperModule();
            services.RegisterHelpersModule();
            services.RegisterCommandsModule();
            services.RegisterQueriesModule();
        }
    }
}