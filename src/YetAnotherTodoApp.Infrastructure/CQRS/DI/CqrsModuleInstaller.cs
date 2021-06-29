using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Application.Queries;

namespace YetAnotherTodoApp.Infrastructure.CQRS.DI
{
    public static class CqrsModuleInstaller
    {
        public static void RegisterCrqsModule(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        }
    }
}
