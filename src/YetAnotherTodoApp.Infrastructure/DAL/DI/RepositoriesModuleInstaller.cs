using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Domain.Repositories;
using YetAnotherTodoApp.Infrastructure.DAL.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    internal static class RepositoriesModuleInstaller
    {
        internal static void RegisterRepositoriesModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
        }
    }
}