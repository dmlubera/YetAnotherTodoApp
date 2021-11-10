using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Domain.Repositories;
using YetAnotherTodoApp.Infrastructure.DAL.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    internal static class RepositoriesModuleInstaller
    {
        internal static void RegisterRepositoriesModule(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITodoListRepository, TodoListRepository>();
            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<ITodoTaskRepository, TodoTaskRepository>();
        }
    }
}