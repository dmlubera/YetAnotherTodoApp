using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Infrastructure.Auth.DI;
using YetAnotherTodoApp.Infrastructure.Cache.DI;
using YetAnotherTodoApp.Infrastructure.CQRS.DI;
using YetAnotherTodoApp.Infrastructure.DAL.Decorators;
using YetAnotherTodoApp.Infrastructure.DAL.DI;

namespace YetAnotherTodoApp.Infrastructure.DI
{
    public static class InfrastructureModuleInstaller
    {
        public static void RegisterInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddYetAnotherTodoAppDbContext(configuration);
            services.RegisterAuthModule();
            services.RegisterCacheModule();
            services.RegisterCrqsModule();
            services.RegisterRepositoriesModule();
            services.AddUnitOfWork();
        }

        public static void AddTransactionalDecorator(this IServiceCollection services)
            => services.TryDecorate(typeof(ICommandHandler<>), typeof(TransactionalCommandHandlerDecorator<>));
    }
}