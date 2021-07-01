using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Infrastructure.Auth.DI;
using YetAnotherTodoApp.Infrastructure.Cache.DI;
using YetAnotherTodoApp.Infrastructure.CQRS.DI;
using YetAnotherTodoApp.Infrastructure.DAL.DI;

namespace YetAnotherTodoApp.Infrastructure.DI
{
    public static class InfrastructureModuleInstaller
    {
        public static void RegisterInfrastructureModule(this IServiceCollection services)
        {
            services.RegisterAuthModule();
            services.RegisterCacheModule();
            services.RegisterCrqsModule();
            services.RegisterRepositoriesModule();
        }
    }
}